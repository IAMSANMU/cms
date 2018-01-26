using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imow.Framework.Redis
{
    public class RedisDb : IDisposable
    {
        /*copyright@2013 All Rights Reserved
         * Author:Mars
         * Date:2013.08.27
         * QQ:258248340
         * servicestack.redis为github中的开源项目
         * redis是一个典型的k/v型数据库
         * redis共支持五种类型的数据 string,list,hash,set,sortedset
         * 
         * string是最简单的字符串类型
         * 
         * list是字符串列表，其内部是用双向链表实现的，因此在获取/设置数据时可以支持正负索引
         * 也可以将其当做堆栈结构使用
         * 
         * hash类型是一种字典结构，也是最接近RDBMS的数据类型，其存储了字段和字段值的映射，但字段值只能是
         * 字符串类型，散列类型适合存储对象，建议使用对象类别和ID构成键名，使用字段表示对象属性，字
         * 段值存储属性值，例如：car:2 price 500 ,car:2  color black,用redis命令设置散列时，命令格式
         * 如下：HSET key field value，即key，字段名，字段值
         * 
         * set是一种集合类型，redis中可以对集合进行交集，并集和互斥运算
         *           
         * sorted set是在集合的基础上为每个元素关联了一个“分数”，我们能够
         * 获得分数最高的前N个元素，获得指定分数范围内的元素，元素是不同的，但是"分数"可以是相同的
         * set是用散列表和跳跃表实现的，获取数据的速度平均为o(log(N))
         * 
         * 需要注意的是，redis所有数据类型都不支持嵌套
         * redis中一般不区分插入和更新操作，只是命令的返回值不同
         * 在插入key时，如果不存在，将会自动创建
         * 
         * 在实际生产环境中，由于多线程并发的关系，建议使用连接池，本类只是用于测试简单的数据类型
         */

        /*
         * 以下方法为基本的设置数据和取数据
         */

        //public static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        //{
        //    //支持读写分离，均衡负载
        //    return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
        //    {
        //        MaxWritePoolSize = 5,//“写”链接池链接数
        //        MaxReadPoolSize = 5,//“写”链接池链接数
        //        AutoStart = true,
        //    });
        //}

        private RedisClient redisCli = null;

        public RedisDb(RedisClient redisClient)
        {
            redisCli = redisClient;
        }

        public RedisDb()
        {
        }

        //public PooledRedisClientManager pool = null;

        ///// <summary>
        ///// 创建客户端连接池
        ///// </summary>
        ///// <param name="readWriteHosts"></param>
        ///// <param name="readOnlyHosts"></param>
        //public  void CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        //{
        //    //支持读写分离，均衡负载
        //    pool=new  PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
        //    {
        //        MaxWritePoolSize = 5,//“写”链接池链接数
        //        MaxReadPoolSize = 5,//“写”链接池链接数
        //        AutoStart = true,
        //    });
        //}

        ///// <summary>
        ///// 获取连接池客户端
        ///// </summary>
        ///// <returns></returns>
        //public RedisDb GetPoolRedisClient()
        //{
        //    redisCli=(RedisClient)pool.GetClient();
        //    return this;
        //}

        //public void SetPassword(string pass)
        //{
        //    redisCli.Password = pass;
        //}

        public RedisClient GetClient()
        {
            return redisCli;
        }

        ///// <summary>
        ///// 建立redis长连接
        ///// </summary>
        ///// 将此处的IP换为自己的redis实例IP，如果设有密码，第三个参数为密码，string 类型
        //public void CreateClient(string hostIP, int port, string password)
        //{
        //    if (redisCli == null)
        //    {
        //        redisCli = new RedisClient(hostIP, port);
        //        if (!string.IsNullOrEmpty(password))
        //        {
        //            redisCli.Password = password;
        //        }
        //    }

        //}
        //public void CreateClient(string hostIP, int port)
        //{
        //    if (redisCli == null)
        //    {
        //        redisCli = new RedisClient(hostIP, port);
        //    }

        //}

        //private static RedisClient redisCli = new RedisClient("192.168.101.165", 6379, "123456");
        /// <summary>
        /// 获取key,返回string格式
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getValueString(string key)
        {
            string value = redisCli.Get<string>(key);
            return value;
        }

        /// <summary>
        /// 获取key,返回byte[]格式
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] getValueByte(string key)
        {

            byte[] value = redisCli.Get(key);
            return value;
        }

        /// <summary>
        /// 获取Key,返回泛型T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            T Value = redisCli.Get<T>(key);
            return Value;
        }

        /// <summary>
        /// 设置一个值 key值重复则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, byte[] value)
        {
            return redisCli.Set(key, value);
        }

        /// <summary>
        /// 设置一个值 key值重复则覆盖
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">value值</param>
        /// <param name="expiresAt">过期时间</param>
        /// <returns></returns>
        public bool SetValue(string key, byte[] value, DateTime expiresAt)
        {
            return redisCli.Set(key, value, expiresAt);
        }

        /// <summary>
        /// 设置一个值 key值重复则覆盖
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">value值</param>
        /// <param name="timespan">过期时间</param>
        /// <returns></returns>
        public bool SetValue(string key, byte[] value, TimeSpan timespan)
        {
            return redisCli.Set(key, value, timespan);
        }
        /// <summary>
        /// 设置一个值 Key值重复则覆盖
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue<T>(string key, T value)
        {
            return redisCli.Set<T>(key, value);
        }

        /// <summary>
        /// 设置一个值 key值重复则覆盖
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key值</param>
        /// <param name="value">value值</param>
        /// <param name="expiresAt">过期时间</param>
        /// <returns></returns>
        public bool SetValue<T>(string key, T value, DateTime expiresAt)
        {
            return redisCli.Set<T>(key, value, expiresAt);
        }

        /// <summary>
        /// 设置一个值 key值重复则覆盖
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">value值</param>
        /// <param name="timespan">过期时间</param>
        /// <returns></returns>
        public bool SetValue<T>(string key, T value, TimeSpan timespan)
        {
            if (typeof(T) == typeof(string))
            {
                try
                {
                    redisCli.SetEntry(key, value as string, timespan);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return redisCli.Set<T>(key, value, timespan);
        }

        /// <summary>
        /// 获得某个hash型key下的所有字段
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public List<string> GetHashFields(string hashId)
        {
            List<string> hashFields = redisCli.GetHashKeys(hashId);
            return hashFields;
        }
        /// <summary>
        /// 获得某个hash型key下的所有值
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public List<string> GetHashValues(string hashId)
        {
            List<string> hashValues = redisCli.GetHashKeys(hashId);
            return hashValues;
        }


        /// <summary>
        /// 获得hash型key某个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        public string GetHashField(string key, string field)
        {
            string value = redisCli.GetValueFromHash(key, field);
            return value;
        }
        /// <summary>
        /// 设置hash型key某个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void SetHashField(string key, string field, string value)
        {
            redisCli.SetEntryInHash(key, field, value);
        }
        /// <summary>
        ///使某个字段增加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public void SetHashIncr(string key, string field, int incre)
        {
            redisCli.IncrementValueInHash(key, field, incre);
        }

        /// <summary>
        /// hash表批量增加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public void HMSet(string key, List<string> fields, List<object> values)
        {
            byte[][] byteKeys = new byte[fields.Count][];
            byte[][] byteValues = new byte[fields.Count][];
            for (int i = 0; i < fields.Count; i++)
            {
                byteKeys[i] = Imow.Framework.Db.LibConvert.StrToBytes(fields[i]);
                byteValues[i] = Imow.Framework.Db.LibConvert.ObjToBytes(values[i]);
            }
            redisCli.HMSet(key, byteKeys, byteValues);
        }

        /// <summary>
        /// hash表批量获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        public byte[][] HMGet(string key, List<string> fields)
        {
            byte[][] byteKeys = new byte[fields.Count][];
            for (int i = 0; i < fields.Count; i++)
            {
                byteKeys[i] = Imow.Framework.Db.LibConvert.StrToBytes(fields[i]);
            }
            return redisCli.HMGet(key, byteKeys);
        }

        /// <summary>
        /// 向list类型数据添加成员，向列表底部(右侧)添加
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="list"></param>
        public void AddItemToListRight(string list, string item)
        {
            redisCli.AddItemToList(list, item);
        }
        /// <summary>
        /// 向list类型数据添加成员，向列表顶部(左侧)添加
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public void AddItemToListLeft(string list, string item)
        {
            redisCli.LPush(list, Encoding.Default.GetBytes(item));
        }

        /// <summary>
        /// 将多个值添加至 redis  List中
        /// </summary>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <param name="seconds"></param>
        public void AddRangeToList(string list, List<string> items, int seconds)
        {
            //using (IRedisTransaction trans = redisCli.CreateTransaction())
            //{
            //    try
            //    {
            //        trans.QueueCommand(r => r.AddRangeToList(list, items));
            //        trans.QueueCommand(r => r.ExpireEntryAt(list, dateTime));
            //        trans.Commit();
            //    }
            //    catch
            //    {
            //        trans.Rollback();
            //    }
            //}
            redisCli.AddRangeToList(list, items);
            redisCli.Expire(list, seconds);

        }

        /// <summary>
        /// 将多个值添加至 redis  List中
        /// </summary>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <param name="dateTime"></param>
        public void ReAddRangeToList(string list, List<string> items, int seconds)
        {
            redisCli.Remove(list);
            redisCli.AddRangeToList(list, items);
            redisCli.Expire(list, seconds);
        }

        /// <summary>
        /// 将多个值添加至 redis  List中
        /// </summary>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <param name="dateTime"></param>
        public void ReAddRangeToList(string list, List<string> items)
        {
            redisCli.Remove(list);
            redisCli.AddRangeToList(list, items);
        }
        /// <summary>
        /// 从list类型数据读取所有成员
        /// </summary>
        public List<string> GetAllItems(string list)
        {
            List<string> listMembers = redisCli.GetAllItemsFromList(list);
            return listMembers;
        }
        /// <summary>
        /// 从list类型数据指定索引处获取数据，支持正索引和负索引
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string GetItemFromList(string list, int index)
        {
            string item = redisCli.GetItemFromList(list, index);
            return item;
        }
        /// <summary>
        /// 从列表中批量获取数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="values"></param>
        public List<string> GetRangeToList(string list, int sindex, int eindex)
        {
            List<string> lr = redisCli.GetRangeFromList(list, sindex, eindex);
            return lr;
        }

        /// <summary>
        /// 从列表中删除指定数量的元素
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long LRemove(string list, int count, string value)
        {
            return redisCli.LRem(list, count, Imow.Framework.Db.LibConvert.StrToBytes(value));
        }

        /// <summary>
        /// 向集合中添加数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="set"></param>
        public void GetItemToSet(string item, string set)
        {
            redisCli.AddItemToSet(item, set);
        }
        /// <summary>
        /// 获得集合中所有数据
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public HashSet<string> GetAllItemsFromSet(string set)
        {
            HashSet<string> items = redisCli.GetAllItemsFromSet(set);
            return items;
        }
        /// <summary>
        /// 获取fromSet集合和其他集合不同的数据
        /// </summary>
        /// <param name="fromSet"></param>
        /// <param name="toSet"></param>
        /// <returns></returns>
        public HashSet<string> GetSetDiff(string fromSet, params string[] toSet)
        {
            HashSet<string> diff = redisCli.GetDifferencesFromSet(fromSet, toSet);
            return diff;
        }
        /// <summary>
        /// 获得所有集合的并集
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public HashSet<string> GetSetUnion(params string[] set)
        {
            HashSet<string> union = redisCli.GetUnionFromSets(set);
            return union;
        }
        /// <summary>
        /// 获得所有集合的交集
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public HashSet<string> GetSetInter(params string[] set)
        {
            HashSet<string> inter = redisCli.GetIntersectFromSets(set);
            return inter;
        }
        /// <summary>
        /// 向有序集合中添加元素
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public void AddItemToSortedSet(string set, string value, long score)
        {
            redisCli.AddItemToSortedSet(set, value, score);
        }

        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的降序排列
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long GetItemIndexInSortedSetDesc(string set, string value)
        {
            long index = redisCli.GetItemIndexInSortedSetDesc(set, value);
            return index;
        }
        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的升序排列
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long GetItemIndexInSortedSet(string set, string value)
        {
            long index = redisCli.GetItemIndexInSortedSet(set, value);
            return index;
        }
        /// <summary>
        /// 获得有序集合中某个值得分数
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public double GetItemScoreInSortedSet(string set, string value)
        {
            double score = redisCli.GetItemScoreInSortedSet(set, value);
            return score;
        }
        /// <summary>
        /// 获得有序集合中，某个排名范围的所有值
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginRank"></param>
        /// <param name="endRank"></param>
        /// <returns></returns>
        public List<string> GetRangeFromSortedSet(string set, int beginRank, int endRank)
        {
            List<string> valueList = redisCli.GetRangeFromSortedSet(set, beginRank, endRank);
            return valueList;
        }
        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，升序
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public List<string> GetRangeFromSortedSet(string set, double beginScore, double endScore)
        {
            List<string> valueList = redisCli.GetRangeFromSortedSetByHighestScore(set, beginScore, endScore);
            return valueList;
        }
        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，降序
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public List<string> GetRangeFromSortedSetDesc(string set, double beginScore, double endScore)
        {
            List<string> vlaueList = redisCli.GetRangeFromSortedSetByLowestScore(set, beginScore, endScore);
            return vlaueList;
        }


        /// <summary>
        /// 删除一个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Delete(string key)
        {
            return redisCli.Del(key);
        }

        /// <summary>
        /// 删除一批key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public long Deletes(string[] keys)
        {
            return redisCli.Del(keys);
        }


        public long Incr(string key)
        {
            return redisCli.Incr(key);
        }
        
        public void CloseClient()
        {
            if (redisCli != null)
                redisCli.Quit();
        }

        public void Dispose()
        {
            if (redisCli != null)
            {
                redisCli.Dispose();
            }
        }

        /// <summary>
        /// 批量数据执行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="onSuccessCallback"></param>
        public void QueueCommand(Action<RedisDb> command, Action onSuccessCallback)
        {
            using (var pipeline = redisCli.CreatePipeline())
            {
                pipeline.QueueCommand(m =>
                {
                    command.Invoke(this);
                }, onSuccessCallback);
                pipeline.Flush();
            }
        }

    }
}

