using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Bichebot.Modules.Survey
{
    public class SurveyModule : StatefulBaseModule<SurveyState>
    {
        private readonly List<string> questions;

        private readonly List<string> possibleAnswers = new List<string>
        {
            "1️⃣", "2️⃣", "3️⃣", "4️⃣", "5️⃣"
        };
        
        public SurveyModule(IBotCore core, Func<SurveyState> createDefault, List<string> questions) : base(core, createDefault)
        {
            this.questions = questions;
        }

        protected override async Task HandleReactionAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
                return;
            
            if (!possibleAnswers.Contains(reaction.Emote.Name))
                return;
            
            var state = GetState(reaction.UserId);
            if (reaction.MessageId == state.LastMessageId) // && possibleAnswers.Contains(reaction.Emote.Name))
            {
                try
                {
                    if (state.CurrentQuestion - 1 != questions.Count)
                        state.Answers.Add(new UserAnswer(questions[state.CurrentQuestion - 1], reaction.Emote.Name));
                    await AskNextQuestion(reaction.User.Value).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        protected override async Task HandleMessageAsync(SocketMessage message)
        {
            if (message.Content != "/start")
                return;

            InitSurvey(message.Author);
            await AskNextQuestion(message.Author).ConfigureAwait(false);
        }

        private void InitSurvey(IUser user)
        {
            var state = SetState(user.Id, new SurveyState());
            state.Status = SurveyStatus.InProgress;
            state.CurrentQuestion = 0;
        }

        private async Task AskNextQuestion(IUser user)
        {
            var state = GetState(user.Id);
            if (state.CurrentQuestion == questions.Count)
            {
                state.Status = SurveyStatus.Done;
                state.LastMessageId = 0;
                File.WriteAllText($"{user.Username}{DateTime.Now.ToString("yyyy-M-d-hh")}", 
                    JsonConvert.SerializeObject(state.Answers.Select(a => new UserAnswer(a.Question, 
                        (possibleAnswers.IndexOf(a.Answer) + 1).ToString()))));
                await user.SendMessageAsync(string.Join("\n", state.Answers.Select(a => $"{a.Question}:{a.Answer}")))
                    .ConfigureAwait(false);
                return;
            }

            var message = await user.SendMessageAsync( Core.ToEmojiString(questions[state.CurrentQuestion]) + " (" + questions[state.CurrentQuestion] + ")")
                .ConfigureAwait(false);

            await message.AddReactionsAsync(possibleAnswers.Select(e => new Emoji(e)).ToArray(), RequestOptions.Default)
                .ConfigureAwait(false);
            state.LastMessageId = message.Id;
            
            state.CurrentQuestion++;
        }
    }
}