﻿namespace GentleBlossom_BE.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
