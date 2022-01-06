﻿using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Extract1
{
    public static class Colors
    {
        public const int Argb1555 = 0x0e;
        public const int Argb4444 = 0x0f;
        public const int Argb8888 = 0X10;

        /// <summary>
        ///     将所有ARGB类型的数据转换为ARGB_8888的字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static void ReadColor(Stream stream, int bits, Color32[] target, int index)
        {
            byte a = 0;
            byte r = 0;
            byte g = 0;
            byte b = 0;

            byte[] bf;
            if (bits == Argb8888)
            {
                stream.Read(4, out bf);
                b = bf[0];
                g = bf[1];
                r = bf[2];
                a = bf[3];
            }
            else
            {
                stream.Read(2, out bf);
                switch (bits)
                {
                    case Argb1555:
                        a = (byte)(bf[1] >> 7);
                        r = (byte)((bf[1] >> 2) & 0x1f);
                        g = (byte)((bf[0] >> 5) | ((bf[1] & 3) << 3));
                        b = (byte)(bf[0] & 0x1f);
                        a = (byte)(a * 0xff);
                        r = (byte)((r << 3) | (r >> 2));
                        g = (byte)((g << 3) | (g >> 2));
                        b = (byte)((b << 3) | (b >> 2));
                        break;
                    case Argb4444:
                        a = (byte)(bf[1] & 0xf0);
                        r = (byte)((bf[1] & 0xf) << 4);
                        g = (byte)(bf[0] & 0xf0);
                        b = (byte)((bf[0] & 0xf) << 4);
                        break;
                }
            }
            target[index] = new Color32(r, g, b, a);
        }

        public static void ReadColor(Stream stream, ColorBits bits, Color32[] target, int offset)
        {
            ReadColor(stream, Convert.ToInt32(bits), target, offset);
        }

        public static Color32[] ReadColor(Stream stream, int bits)
        {
            var target = new Color32[1];
            ReadColor(stream, bits, target, 0);
            return target;
        }

        /// <summary>
        ///     读取一个指定数量的色表
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<Color32> ReadPalette(Stream stream, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var data = new byte[4];
                stream.Read(data);
                yield return new Color32(data[3], data[0], data[1], data[2]);
            }
        }

        public static Color32[] ReadColor(Stream stream, ColorBits bits)
        {
            return ReadColor(stream, (int)bits);
        }



        /// <summary>
        ///     将ARGB1555和ARGB4444转换为ARGB8888
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        /// <param name="bits"></param>
        public static void WriteColor(Stream stream, byte[] data, ColorBits bits)
        {
            if (bits == ColorBits.ARGB_8888)
            {
                stream.Write(data);
                return;
            }
            var a = data[3];
            var r = data[2];
            var g = data[1];
            var b = data[0];
            var left = 0;
            var right = 0;
            switch (bits)
            {
                case ColorBits.ARGB_1555:
                    a = (byte)(a >> 7);
                    r = (byte)(r >> 3);
                    g = (byte)(g >> 3);
                    b = (byte)(b >> 3);
                    left = (byte)(((g & 7) << 5) | b);
                    right = (byte)((a << 7) | (r << 2) | (g >> 3));
                    break;
                case ColorBits.ARGB_4444:
                    left = g | (b >> 4);
                    right = a | (r >> 4);
                    break;
            }
            stream.WriteByte((byte)left);
            stream.WriteByte((byte)right);
        }

        /// <summary>
        ///     写入一个颜色
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="color"></param>
        /// <param name="bits"></param>
        public static void WriteColor(this Stream stream, Color32 color, ColorBits bits)
        {
            byte[] data = { color.r, color.g, color.b, color.a };
            WriteColor(stream, data, bits);
        }

        //public static void WritePalette(Stream stream, IEnumerable<Color> table)
        //{
        //    var list = table.ToList();
        //    foreach (var color in list)
        //    {
        //        var data = new[] { color.R, color.G, color.B, color.A };
        //        stream.Write(data);
        //    }
        //}

        //public static string ToHexString(this Color color)
        //{
        //    var val = color.ToArgb();
        //    var str = val.ToString("x2");
        //    for (var i = str.Length; i < 8; i++)
        //    {
        //        str = "0" + str;
        //    }
        //    return $"#{str}";
        //}
    }
}