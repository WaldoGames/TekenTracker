﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes
{
    public class FileChecker
    {
        int UploadSizeLimitInMb = 10;

        public FileUploadPreCheckValue TestFile(IFormFile Ifile)
        {
            using (var memoryStream = new MemoryStream())
            {
                Ifile.CopyTo(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length > UploadSizeLimitInMb * 1048576)
                {
                    return FileUploadPreCheckValue.TooLarge;
                }

                byte[] file = memoryStream.ToArray();
                if (!CheckImageAllFileSignatures(file))
                {
                    return FileUploadPreCheckValue.NoValidFIleType;
                }

                return FileUploadPreCheckValue.Accepted;

            }

        }
        public bool CheckImageAllFileSignatures(byte[] file)
        {
            List<byte[]> Signatures = new List<byte[]>();
            Signatures.Add(StringToByteArray("89504E470D0A1A0A"));
            Signatures.Add(StringToByteArray("FFD8FFE000104A4649460001"));
            Signatures.Add(StringToByteArray("FFD8FFEE"));
            Signatures.Add(StringToByteArray("FFD8FFE0"));
            Signatures.Add(StringToByteArray("0000000C6A5020200D0A870A"));
            Signatures.Add(StringToByteArray("FF4FFF51"));
            Signatures.Add(StringToByteArray("474946383761"));
            Signatures.Add(StringToByteArray("474946383961"));

            //a jpeg file signature dived in 2 parts. only dual part signature i have. if more added in the future program beter solution
            byte[] special1 = StringToByteArray("FFD8FFE1");
            byte[] special2 = StringToByteArray("457869660000");

            if (CheckImageFileSignature(file, special1) == true && CheckImageFileSignature(file, special2, 6) == true)

                foreach (var signature in Signatures)
                {
                    if (CheckImageFileSignature(file, signature) == true)
                    {
                        return true;
                    }
                }
            return false;
        }

        public bool CheckImageFileSignature(byte[] file, byte[] Signature, int offset = 0)
        {
            if (Signature.Length > file.Length)
            {
                return false;
            }

            for (int i = 0; i < Signature.Length; i++)
            {
                if (file[i + offset] != Signature[i])
                {
                    return false;
                }
            }
            return true;
        }
        //source: https://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

    }

    public enum FileUploadPreCheckValue
    {
        Accepted, TooLarge, NoValidFIleType
    }
}
