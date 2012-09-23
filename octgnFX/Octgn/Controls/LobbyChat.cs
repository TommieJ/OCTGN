﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LobbyChat.cs" company="OCTGN">
//   GNU Stuff
// </copyright>
// <summary>
//   Control specifically for the lobby chat room
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Octgn.Controls
{
    using Skylabs.Lobby;

    /// <summary>
    ///     Control specifically for the lobby chat room.
    /// </summary>
    public class LobbyChat : ChatControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LobbyChat"/> class.
        /// </summary>
        public LobbyChat()
        {
            Program.OctgnInstance.LobbyClient.Chatting.OnCreateRoom += this.Chatting_OnCreateRoom;
        }

        /// <summary>
        /// When a Lobby creates a chat room.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="createdroom">
        /// The created room.
        /// </param>
        private void Chatting_OnCreateRoom(object sender, NewChatRoom createdroom)
        {
            if (createdroom.GroupUser == null || createdroom.GroupUser.User.User != "lobby" || this.Room != null)
            {
                return;
            }

            this.SetRoom(createdroom);
        }
    }
}