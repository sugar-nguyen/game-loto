using Loto.Handle;
using Loto.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loto.Hubs
{
    public class ChatHub : Hub<IChatHub>
    {
        static List<Room> _Rooms = new List<Room>();
        const short MEMBER_IN_ROOM = 2;
        const short WIN_NUMBER = 5;
        const int TIEN_CUOC = 20000;

        public async Task UserConnected(string username, string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                string id = Context.ConnectionId;
                string[] arrId = _Rooms.Any() ? _Rooms.Select(x => x.RoomId).ToArray() : null;
                roomId = await GeneratorId(arrId);
                var member = new Member() { UserId = id, RoomId = roomId, UserName = username };
                var curRoom = new Room();
                curRoom.RoomId = roomId;
                curRoom.Members.Add(member);
                _Rooms.Add(curRoom);

                await Groups.AddToGroupAsync(id, roomId);
                await Clients.Caller.onUserCreateRoom(roomId, member);
            }
            else
            {
                var curRoom = _Rooms.SingleOrDefault(x => x.RoomId.Trim().ToUpper() == roomId.Trim().ToUpper());
                if (curRoom != null)
                {
                    if (curRoom.Members.Count == MEMBER_IN_ROOM)
                    {
                        await Clients.Caller.overflowMember();
                    }
                    else
                    {
                        var member = curRoom.Members.SingleOrDefault(x => x.UserName.ToUpper() == username.ToUpper());
                        if (member == null)
                        {
                            member = new Member() { RoomId = curRoom.RoomId, UserName = username, UserId = Context.ConnectionId };
                            curRoom.Members.Add(member);

                            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                            await Clients.Clients(curRoom.Members.First().UserId).addUserToRoom(roomId, curRoom.Members.First(), member);
                            await Clients.Caller.addUserToRoom(roomId, member, curRoom.Members.First(), true);
                        }
                        else
                        {
                            await Clients.Caller.userNameExists();
                        }
                    }

                }
                else
                {
                    await Clients.Caller.roomIdNotExists();
                }

            }

        }
        public async Task UserPickNumber(string number)
        {
            var member = GetClientCaller();
            var room = GetRoomCaller(member.RoomId);
            var firstObj = room.Members.First();
            if (firstObj.UserId == member.UserId) firstObj = room.Members.Last();
            if (member != null)
            {
                var isWin = PickNumberAndCheckWin(member, int.Parse(number));
                await Clients.Clients(firstObj.UserId).onUserPickNumber(int.Parse(number));
                if (isWin)
                {
                    UpdateTienCuoc(member, firstObj);
                    await Clients.Caller.onUserWin(member, firstObj);
                    await Clients.Client(firstObj.UserId).onUserWin(member, firstObj, true);
                }
            }
        }

        private bool PickNumberAndCheckWin(Member member, int number)
        {
            foreach (var item in member.ListCheckingBingo)
            {
                foreach (var num in item.ls)
                {
                    if (num.Number == number)
                    {
                        num.Pick = true;
                        if (item.ls.Count(x => x.Pick == true) == WIN_NUMBER)
                        {
                            return true;
                        }
                        break;
                    }
                }
            }
            return false;
        }

        private void UpdateTienCuoc(Member memberWin, Member memberLost)
        {

            if (memberLost.Money >= TIEN_CUOC)
            {
                memberWin.Money += TIEN_CUOC;
                memberLost.Money -= TIEN_CUOC;
            }
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string id = Context.ConnectionId;
            foreach (var room in _Rooms)
            {
                var member = room.Members.SingleOrDefault(x => x.UserId == id);
                if (member != null)
                {
                    room.Members.Remove(member);
                    if (room.Members.Count == 0)
                    {
                        _Rooms.Remove(room);
                        break;
                    }
                    else
                    {
                        await Groups.RemoveFromGroupAsync(id, room.RoomId);
                        await Clients.Groups(room.RoomId).onUserOutRoom(member.UserName);
                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        private Member GetClientCaller()
        {
            string id = Context.ConnectionId;
            foreach (var room in _Rooms)
            {
                var member = room.Members.SingleOrDefault(x => x.UserId == id);
                if (member != null)
                {
                    return member;
                }
            }
            return null;
        }
        private Room GetRoomCaller(string roomId)
        {
            return _Rooms.SingleOrDefault(x => x.RoomId == roomId);
        }
        private async Task<string> GeneratorId(string[] arr)
        {
            string myString = "0123456789";
            Random random = new Random();

            var result = new string(Enumerable.Repeat(myString, 5).Select(s => s[random.Next(s.Length)]).ToArray());

            if (arr != null && Array.Exists(arr, x => x == result))
            {
                return await GeneratorId(arr);
            }
            return result;
        }
    }
}
