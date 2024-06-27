using System;
using System.Collections.Generic;

namespace InHues.Domain.DTO.Custom
{
    public class FileResponse
    {
        private Guid _Id;

        public FileResponse()
        {
            _Id = Guid.NewGuid();
        }

        public string Id { get { return _Id.ToString(); } }
        public string FileName { get; set; }
        public float Size { get; set; }
        public string Base64 { get; set; }
        public string ContentType { get; set; }
        public byte[] ByteArray { get; set; }
        public bool IsFinalized { get; set; } = false;
        public List<string> RequestExceptions { get; set; } = new();

        public string GetFileType()
        {
            if (string.IsNullOrEmpty(ContentType) || !ContentType.Contains('/')) return string.Empty;
            return ContentType.Split("/")[1];
        }
    }
}
