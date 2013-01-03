// <copyright file="MessageDeliveryStatus.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Threading.Tasks;

namespace ATT.Services
{
	/// <summary>
	/// Enumeration with all supported message delivery statuses - currently Initial, DeliveredToNetwork, DeliveredToTerminal, DeliveryImpossible, and Error.
	/// </summary>
	public enum MessageDeliveryStatus
	{
		/// <summary>
		/// Initial status.
		/// </summary>
		Initial,

		/// <summary>
		/// Delivered to the network but not yet delivered to the Handset.
		/// </summary>
		DeliveredToNetwork,

		/// <summary>
		/// Successfully delivered to the Handset of the destination mobile number.
		/// </summary>
		DeliveredToTerminal,

		/// <summary>
		/// Unsuccessful delivery; the message could not be delivered before it expired.
		/// </summary>
		DeliveryImpossible,

		/// <summary>
		/// Was thrown exception
		/// </summary>
		Error,
	}
}
