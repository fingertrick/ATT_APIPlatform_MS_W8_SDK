// <copyright file="FileType.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

namespace ATT.Controls
{
	/// <summary>
	/// This is an enumeration that contains the file types supported by AT&amp;T controls.
	/// </summary>
	public enum FileType
	{
		/// <summary>
		/// Any file type
		/// </summary>
		Any,
		
		/// <summary>
		/// Audio file type (e.g. “.mp3”, “.wav”).
		/// </summary>
		Audio,
		
		/// <summary>
		/// Video file type (e.g. “.wmv”, “.avi”).
		/// </summary>
		Video,

		/// <summary>
		/// Picture file type (e.g. “.jpg”, “.png”).
		/// </summary>
		Picture
	}
}
