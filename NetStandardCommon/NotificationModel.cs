namespace NetStandardCommon
{
    /// <summary> お知らせ機能モデル </summary>
    public class NotificationItemModel
    {
        /// <summary>お知らせID</summary>
        public int Id { get; set; }
        /// <summary> タイトル </summary>
        public string Title { get; set; }
        /// <summary>お知らせリンク</summary>
        public string URL { get; set; }
        /// <summary> 既読</summary>
        public bool IsReaded { get; set; }
        /// <summary> 重要なお知らせ </summary>
        public bool IsImportant { get; set; }
    }
}
