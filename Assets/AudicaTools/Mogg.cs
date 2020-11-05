using System;
using System.IO;
namespace AudicaTools
{
    public class Mogg
    {
        public byte[] bytes;

        public Mogg(Stream stream)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            this.bytes = ms.ToArray();
        }

        public void ExportToOgg(string filePath)
        {
            //ogg export courtesy of the Audica Modding Discord
            byte[] oggStartLocation = new byte[4];

            oggStartLocation[0] = bytes[4];
            oggStartLocation[1] = bytes[5];
            oggStartLocation[2] = bytes[6];
            oggStartLocation[3] = bytes[7];

            int start = BitConverter.ToInt32(oggStartLocation, 0);

            byte[] dst = new byte[bytes.Length - start];
            Array.Copy(bytes, start, dst, 0, dst.Length);
            File.WriteAllBytes(filePath, dst);
        }

        public MemoryStream GetMemoryStream()
        {
            return new MemoryStream(bytes);
        }

        public float[] GetAudioClipData()
        {
            //ogg export courtesy of the Audica Modding Discord
            byte[] oggStartLocation = new byte[4];

            oggStartLocation[0] = bytes[4];
            oggStartLocation[1] = bytes[5];
            oggStartLocation[2] = bytes[6];
            oggStartLocation[3] = bytes[7];

            int start = BitConverter.ToInt32(oggStartLocation, 0);

            byte[] dst = new byte[bytes.Length - start];
            Array.Copy(bytes, start, dst, 0, dst.Length);

            return ConvertByteToFloat(bytes);
            
            //Example Usage:
            //var songData = Audica.[AnyMoggHere].GetAudioClipData();
            //AudioClip audioClip = AudioClip.Create("song", songData.Length, 1, 44100, false, false);
            //audioClip.SetData(songData, 0);
        }


        private float[] ConvertByteToFloat(byte[] array)
        {
            float[] floatArr = new float[array.Length / 4];
            for (int i = 0; i < floatArr.Length; i++)
            {
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(array, i * 4, 4);
                floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
            }
            return floatArr;
        }

    }

}