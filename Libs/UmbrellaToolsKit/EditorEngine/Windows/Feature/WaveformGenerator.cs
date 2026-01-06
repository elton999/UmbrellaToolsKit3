using NAudio.Wave;
using System;
using System.IO;
using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Windows.Feature
{
    public class WaveformData
    {
        public int SampleRate;
        public int Channels;
        public int BucketsPerSecond;
        public float[] MinMax;

        public int BucketCount => MinMax.Length / 2;
    }

    public static class WaveformGenerator
    {
        public static WaveformData Generate(string audioPath, int bucketsPerSecond = 100)
        {
            WaveStream reader;
            if (audioPath.EndsWith(".ogg"))
            {
                reader = new NAudio.Vorbis.VorbisWaveReader(audioPath);
            }
            else
            {
                reader = new MediaFoundationReader(audioPath);
            }

            int sampleRate = reader.WaveFormat.SampleRate;
            int channels = reader.WaveFormat.Channels;

            int samplesPerBucket = sampleRate * channels / bucketsPerSecond;

            var minMax = new List<float>();

            float min = float.MaxValue;
            float max = float.MinValue;
            int count = 0;
            var sampleProvider = reader.ToSampleProvider();

            float[] buffer = new float[4096];
            int read;

            while ((read = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < read; i++)
                {
                    float s = buffer[i];
                    min = Math.Min(min, s);
                    max = Math.Max(max, s);
                    count++; ;

                    if (count >= samplesPerBucket)
                    {
                        minMax.Add(min);
                        minMax.Add(max);
                        min = float.MaxValue;
                        max = float.MinValue;
                        count = 0;
                    }
                }
            }

            return new WaveformData
            {
                SampleRate = sampleRate,
                Channels = channels,
                BucketsPerSecond = bucketsPerSecond,
                MinMax = minMax.ToArray()
            };
        }

        public static void Save(string path, WaveformData data)
        {
            using var bw = new BinaryWriter(File.Create(path));
            bw.Write(data.SampleRate);
            bw.Write(data.Channels);
            bw.Write(data.BucketsPerSecond);
            bw.Write(data.MinMax.Length);
            foreach (var v in data.MinMax)
                bw.Write(v);
        }

        public static WaveformData Load(string path)
        {
            using var br = new BinaryReader(File.OpenRead(path));
            var data = new WaveformData
            {
                SampleRate = br.ReadInt32(),
                Channels = br.ReadInt32(),
                BucketsPerSecond = br.ReadInt32()
            };

            int len = br.ReadInt32();
            data.MinMax = new float[len];
            for (int i = 0; i < len; i++)
                data.MinMax[i] = br.ReadSingle();

            return data;
        }
    }
}