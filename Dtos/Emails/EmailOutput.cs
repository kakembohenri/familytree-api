﻿namespace familytree_api.Dtos.Emails
{
    public class EmailOutput
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string HtmlContent { get; set; } = string.Empty;
    }
}
