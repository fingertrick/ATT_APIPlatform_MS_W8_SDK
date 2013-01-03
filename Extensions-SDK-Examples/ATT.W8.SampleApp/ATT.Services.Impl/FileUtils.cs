// <copyright file="FileUtils.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ATT.Services.Impl
{
	/// <summary>
	/// Static class for file utils
	/// </summary>
	public class FileUtils
	{
		/// <summary>
		/// Reads a binary file, returning a byte array.
		/// </summary>
		/// <param name="file">File to read</param>
		/// <returns>Returns Task as a result of asynchronous operation.
		/// Task result is full file content</returns>
		public static async Task<byte[]> ReadAllBytes(StorageFile file)
		{
			IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
			byte[] buffBytes = new byte[stream.Size];
			IBuffer buffer = buffBytes.AsBuffer();
			await stream.ReadAsync(buffer, (uint)stream.Size, InputStreamOptions.ReadAhead);
			return buffBytes;
		}
	}
}
