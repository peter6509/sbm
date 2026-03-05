//using Confluent.Kafka;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace VolPro.Core.KafkaManager.IService
//{
//    public interface IKafkaConsumer<TKey, TValue> : IDisposable
//    {
//        /// <summary>
//        /// 訂閱回調模式-消費(持續訂閱)
//        /// </summary>
//        /// <param name="Func">回調函數,若配置為非自動提交(默認為否),则通過回調函數的返回值判断是否提交</param>
//        /// <param name="Topic">主题</param>
//        void Consume(Func<ConsumeResult<TKey, TValue>, bool> Func, string Topic);

//        /// <summary>
//        /// 批量訂閱回調模式-消費(持續訂閱)
//        /// </summary>
//        /// <param name="Func">回調函數,若配置為非自動提交(默認為否),则通過回調函數的返回值判断是否提交</param>
//        /// <param name="Topics">主题集合</param>
//        void ConsumeBatch(Func<ConsumeResult<TKey, TValue>, bool> Func, List<string> Topics);

//        /// <summary>
//        /// 批量消費模式-單次消費(消費出當前Kafka缓存的所有數據,並持續監聽 300ms,如无新數據生產,则返回(最多一次消費 100条)
//        /// </summary>
//        /// <param name="Topic">主题</param>
//        /// <param name="TimeOut">持續監聽時间,單位ms 默認值:300ms</param>
//        /// <param name="MaxRow">最多單次消費行數 默認值:100行</param>
//        /// <returns>待消費數據</returns>
//        List<ConsumeResult<TKey, TValue>> ConsumeOnce(string Topic, int TimeOut = 300, int MaxRow = 100);

//        /// <summary>
//        /// 單笔消費模式-單行消費
//        /// </summary>
//        /// <param name="Topic">主题</param>
//        /// <param name="TimeOut">持續監聽時间,單位ms 默認值:300ms</param>
//        /// <returns>待消費數據</returns>
//        ConsumeResult<TKey, TValue> ConsumeOneRow(string Topic, int TimeOut = 300);
//    }
//}
