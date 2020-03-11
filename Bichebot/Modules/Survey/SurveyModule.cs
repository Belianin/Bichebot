using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bichebot.Core;
using Bichebot.Modules.Base;
using Discord;
using Discord.WebSocket;

namespace Bichebot.Modules.Survey
{
    public class SurveyModule : StatefulBaseModule<SurveyState>
    {
        private readonly List<string> questions;

        private readonly ICollection<string> possibleAnswers = new[]
        {
            "one", "two", "three", "four", "five"
        };
        
        public SurveyModule(IBotCore core, Func<SurveyState> createDefault, List<string> questions) : base(core, createDefault)
        {
            //this.questions = questions;
            questions = Core.Guild.Emotes.Select(e => e.Name).ToList();
        }

        protected override async Task HandleReactionAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var state = GetState(reaction.UserId);
            if (reaction.Message.Value.Id == state.LastMessageId) // && possibleAnswers.Contains(reaction.Emote.Name))
            {
                state.Answers.Add(new UserAnswer(questions[state.CurrentQuestion], reaction.Emote.Name));
                await AskNextQuestion(reaction.User.Value).ConfigureAwait(false);
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
                await user.SendMessageAsync(string.Join("\n", state.Answers.Select(a => $"{a.Question}:{a.Answer}")))
                    .ConfigureAwait(false);
                return;
            }
            else
            {
                state.CurrentQuestion++;
            }


            var message = await user.SendMessageAsync(Core.ToEmojiString(questions[state.CurrentQuestion]))
                .ConfigureAwait(false);

            // bug here
            await message.AddReactionsAsync(possibleAnswers.Select(Emote.Parse).ToArray(), RequestOptions.Default)
                .ConfigureAwait(false);

            state.LastMessageId = message.Id;
        }
    }
}