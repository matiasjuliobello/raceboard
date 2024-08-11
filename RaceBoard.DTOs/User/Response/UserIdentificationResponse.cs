﻿using RaceBoard.DTOs.IdentificationType.Response;

namespace RaceBoard.DTOs.User.Response
{
    public class UserIdentificationResponse
    {
        public int Id { get; set; }
        public UserSimpleResponse User { get; set; }
        public string Number { get; set; }
        public IdentificationTypeResponse Type { get; set; }
        public bool IsMain { get; set; }

    }
}