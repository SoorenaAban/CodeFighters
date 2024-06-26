﻿using CodeFighters.Models;

namespace CodeFighters.WebApi.Dto
{
    public class GameOverviewDto
    {
        public GameOverviewDto(GameModel game)
        {
            Id = game.Id;
            PlayerOne = game.PlayerOne.Username;
            PlayerTwo = game.PlayerTwo.Username;
            IsActive = game.IsActive;
            StartTime = game.StartTime;
        }

        public Guid Id { get; set; }
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
        public bool IsActive { get; set; }

        public DateTime StartTime { get; set; }
    }
}
