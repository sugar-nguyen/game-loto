using Loto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loto.Hubs
{
    public interface IChatHub
    {
        Task onUserConnected(string userName, string roomId);
        Task onUserCreateRoom(string roomId, Member user);
        Task addUserToRoom(string roomId, Member caller, Member user, bool isUser2=false);
        Task onUserOutRoom(string username);
        Task userNameExists();
        Task overflowMember();
        Task roomIdNotExists();
        Task createArrGameNumber();
        Task onUserPlayGame(int[] arrayNumber);
        Task onUserRePlayGame();
        Task onUserPlayNewGame(Member user1, Member user2);
        Task onUserPickNumber(int number);
        Task onUserWin(Member memberWin,Member memberLost,bool isUser2=false);
    }
}
