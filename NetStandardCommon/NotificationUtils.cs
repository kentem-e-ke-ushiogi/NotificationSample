using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetStandardCommon
{
    public class NotificationUtils
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
                    Date = item.Timestamp,
                };
                list.Add(model);
            }
            if (important)
                return list.Where(p => p.IsImportant).ToArray();
            return list.ToArray();
        }

        /// <summary> 既読を付ける</summary>
        public static bool SetReaded(int[] ids)
        {
            if (string.IsNullOrEmpty(JsonFolderPath))
                throw new InvalidOperationException("Call Initialize Method");
            bool add = false;
            List<int> list = GetReadedList();
            foreach (int id in ids)
            {
                if (!list.Contains(id))
                {
                    list.Add(id);
                    add = true;
                }
            }
            if (!add)
                return false;

            string filePath = Path.Combine(JsonFolderPath, JsonFileName);
            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(filePath, json);
            return true;
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
                //return new DummyNotificationDTO[0];
                List <DummyNotificationDTO> list = new List<DummyNotificationDTO>();

                var item0 = new DummyNotificationDTO()
                {
                    Id = 1,
                    Title = "サーバーメンテナンス（12/17）のお知らせ",
                    URL = "https://www.kentem.jp/support/20230711_01/",
                    IsImportant = true,
                    Timestamp = new DateTime(2013, 12, 8),
                };
                list.Add(item0);

                var item1 = new DummyNotificationDTO()
                {
                    Id = 2,
                    Title = "【重要】SiteBox大幅バージョンアップにおける注意事項について",
                    URL = "https://www.kentem.jp/support/20230711_01/",
                    IsImportant = true,
                    Timestamp = new DateTime(2023, 8, 22),
                };
                list.Add(item1);

                for (int i = 3; i <= 8; i++)
                {
                    var item = new DummyNotificationDTO()
                    {
                        Id = i,
                        Title = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" + i,
                        URL = "https://www.kentem.jp/support/20230711_01/",
                        IsImportant = false,
                        Timestamp = DateTime.Now,
                    };
                    list.Add(item);
                }
                var item2 = new DummyNotificationDTO()
                {
                    Id = 9,
                    Title = "【重要】お知らせ9",
                    URL = "https://www.kentem.jp/support/20230711_01/",
                    IsImportant = true,
                    Timestamp = DateTime.Now,
                };
                list.Add(item2);
                var item3 = new DummyNotificationDTO()
                {
                    Id = 10,
                    Title = "【重要】お知らせ10",
                    URL = "https://www.kentem.jp/support/20230711_01/",
                    IsImportant = true,
                    Timestamp = DateTime.Now,
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
            public DateTime Timestamp { get; set; }
        }

    }
}
