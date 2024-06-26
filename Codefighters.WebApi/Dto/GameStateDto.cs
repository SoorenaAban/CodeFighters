﻿using CodeFighters.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace CodeFighters.WebApi.Dto
{
    public class GameStateDto
    {
        public GameStateDto(GameModel game)
        {
            Id = game.Id;
            PlayerOneUsername = game.PlayerOne.Username;
            PlayerTwoUsername = game.PlayerTwo.Username;
            PlayerOneHealth = game.PlayerOneHealth;
            PlayerTwoHealth = game.PlayerTwoHealth;
            TurnNumber = game.TurnNumber;
            QuestionCount = game.QuestionCount;
            StartTime = game.StartTime;
            EndTime = game.EndTime;
            IsActive = game.IsActive;
            //CurrentQuestion = game.CurrentQuestion;
        }

        public Guid Id { get; set; }
        public string PlayerOneUsername { get; set; }
        public string PlayerTwoUsername { get; set; }
        public int PlayerOneHealth { get; set; }
        public int PlayerTwoHealth { get; set; }
        public int TurnNumber { get; set; }
        public int QuestionCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}
