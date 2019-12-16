using System;
using System.IO;
using UnityEngine;

namespace PortiaHelper.Core
{
	// This was made by aaro4130 on the Unity forums. Thanks boss!
	// It's been optimized and slimmed down for the purpose of loading Quake 3 TGA textures from memory streams.
	public static class TGALoader
	{
		public static Texture2D LoadTGA(string fileName, bool mipmap = true, bool flipVertical = false, bool flipHorizontal = false) {
			using (var imageFile = File.OpenRead(fileName)) {
				return LoadTGA(imageFile, mipmap, flipVertical, flipHorizontal);
			}
		}

		public static Texture2D LoadTGA(Stream TGAStream, bool mipmap = true, bool flipVertical = false, bool flipHorizontal = false) {
			using (BinaryReader r = new BinaryReader(TGAStream)) {
				// Skip some header info we don't care about.
				// Even if we did care, we have to move the stream seek point to the beginning,
				// as the previous method in the workflow left it at the end.
				r.BaseStream.Seek(12, SeekOrigin.Begin);

				short width = r.ReadInt16();
				short height = r.ReadInt16();
				int bitDepth = r.ReadByte();

				// Skip a byte of header information we don't care about.
				r.BaseStream.Seek(1, SeekOrigin.Current);

				if (bitDepth != 24 && bitDepth != 32) {
					throw new Exception("TGA texture had non 32/24 bit depth.");
				}

				// Read TGA image data
				Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, mipmap);
				Color32[] pulledColors = new Color32[width * height];
				bool readAlpha = bitDepth == 32;

				for (int y = 1; y <= height; ++y) {
					for (int x = 1; x <= width; ++x) {
						byte red = r.ReadByte();
						byte green = r.ReadByte();
						byte blue = r.ReadByte();
						byte alpha = readAlpha ? r.ReadByte() : (byte)255;

						int i = (flipVertical ? (height - y) * width : (y - 1) * width) + (flipHorizontal ? width - x : x - 1);
						pulledColors[i] = new Color32(blue, green, red, alpha);
					}
				}

				texture.SetPixels32(pulledColors);
				texture.Apply();

				return texture;
			}
		}
	}
}
