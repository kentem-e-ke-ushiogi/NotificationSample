using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetStandardCommon
{
    public static class NotificationUtils
    {
        private const string JsonFileName = "NotificationReadedList.json";
        private static string JsonFolderPath = "";
        /// <summary>既読リスト保存先 </summary>
        public static void SetJsonFolderPath(string folderPath)
        {
            JsonFolderPath = folderPath;
        }
        /// <summary> お知らせ一覧取得</summary>
        public static Task<NotificationItemModel[]> GetAllNotifications()
        {
            return GetAllNotifications(false);
        }

        /// <summary> 重要なお知らせ一覧取得</summary>
        public static Task<NotificationItemModel[]> GetImportantNotifications()
        {
            return GetAllNotifications(true);
        }

        private static async Task< NotificationItemModel[]> GetAllNotifications(bool important)
        {
            if (string.IsNullOrEmpty(JsonFolderPath))
                throw new InvalidOperationException("Use SetJsonFolderPath");

            // KoDo:サーバーへのの問い合わせ処理とDTO
            var items = await CreateDummyDatas();
            if (items.Length == 0)
                return new NotificationItemModel[0];

            // 既読リスト
            List<int> readedList = GetReadedList();

            List<NotificationItemModel> list = new List<NotificationItemModel>();
            foreach (var item in items)
            {
                var model = new NotificationItemModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    URL = item.URL,
                    IsImportant = item.IsImportant,
                    IsReaded = readedList.Contains(item.Id),
                };
                list.Add(model);
            }
            if (important)
                return list.Where(p => p.IsImportant).ToArray();
            return list.ToArray();
        }

        /// <summary> 既読を付ける</summary>
        public static void SetReaded(int id)
        {
            if (string.IsNullOrEmpty(JsonFolderPath))
                throw new InvalidOperationException("Use SetJsonFolderPath");

            List<int> list = GetReadedList();
            if (list.Contains(id))
                return;
            list.Add(id);
            string json = JsonConvert.SerializeObject(list);
            string filePath = Path.Combine(JsonFolderPath, JsonFileName);
            File.WriteAllText(filePath, json);
        }

        /// <summary> 未読のお知らせがあるか </summary>
        public static bool ExistNotReaded()
        {
            if (string.IsNullOrEmpty(JsonFolderPath))
                throw new InvalidOperationException("Use SetJsonFolderPath");
            var res = Task.Run(async() =>  await GetAllNotifications(false));
            var readedList = GetReadedList();
            return res.Result.Any( p => !readedList.Contains(p.Id));

        }
        private static List<int> GetReadedList()
        {
            string filePath = Path.Combine(JsonFolderPath, JsonFileName);
            if (!File.Exists(filePath))
                return new List<int>();
            string json = File.ReadAllText(filePath);
            List<int> list = JsonConvert.DeserializeObject<List<int>>(json);
            if (list == null) return new List<int>();
            return list;
        }

        /// <summary> ダミーデータ作成 </summary>
        private static async Task<DummyNotificationDTO[]> CreateDummyDatas()
        {
            var task = await Task.Run(() =>
            {
                List<DummyNotificationDTO> list = new List<DummyNotificationDTO>();
                for (int i = 1; i <= 8; i++)
                {
                    var item = new DummyNotificationDTO()
                    {
                        Id = i,
                        Title = "お知らせ" + i,
                        URL = "https://www.kentem.jp/support/20230711_01/",
                        IsImportant = false,
                    };
                    list.Add(item);
                }
                var item2 = new DummyNotificationDTO()
                {
                    Id = 9,
                    Title = "【重要】お知らせ9",
                    URL = "https://www.kentem.jp/support/20230711_01/",
                    IsImportant = true,
                };
                list.Add(item2);
                var item3 = new DummyNotificationDTO()
                {
                    Id = 10,
                    Title = "【重要】お知らせ10",
                    URL = "https://www.kentem.jp/support/20230711_01/",
                    IsImportant = true,
                };
                list.Add(item3);
                //return new DummyNotificationDTO[0];
                return list.ToArray();
            });
            return task;
        }

        /// <summary>
        /// サーバーからもらう情報
        /// </summary>
        private class DummyNotificationDTO
        {
            /// <summary>お知らせID</summary>
            public int Id { get; set; }
            /// <summary>タイトル</summary>
            public string Title { get; set; }
            /// <summary>URL</summary>
            public string URL { get; set; }
            /// <summary>重要なお知らせ</summary>
            public bool IsImportant { get; set; }
            /// <summary>タイムスタンプ</summary>
            public long Timestamp { get; set; }
        }

    }
}
