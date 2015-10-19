﻿using System;

namespace ZSharp.Framework.Redis
{
    public class RedisLock : IRedisLock
    {
        private readonly IRedisWrapper redisWrapper;

        public RedisLock()
        {
            this.redisWrapper = RedisFactory.GetRedisWrapper();
        }

        public bool Lock(string key, TimeSpan timeOut)
        {
            var expireTime = DateTimeOffset.UtcNow.Add((TimeSpan)timeOut);
            string lockString = (expireTime.ToUnixTimeMilliseconds() + 1).ToString();

            //當沒有正常釋放時，在原來過期時間基礎上加30天讓其會自動釋放；
            var autoReleaseSpan = timeOut.Add(TimeSpan.FromDays(30));  

            //Try to set the lock, if it does not exist this will succeed and the lock is obtained
            var isOk = redisWrapper.SetIfNotExists(key, lockString, autoReleaseSpan);
            if (isOk)
            {
                return true;
            }

            //If we've gotten here then a key for the lock is present. This could be because the lock is
            //correctly acquired or it could be because a client that had acquired the lock crashed (or didn't release it properly).
            //Therefore we need to get the value of the lock to see when it should expire
            string lockExpireString = redisWrapper.Get(key).ToString();
            long lockExpireTime;
            if (!long.TryParse(lockExpireString, out lockExpireTime))
            {
                return false;
            }
            //If the expire time is greater than the current time then we can't let the lock go yet
            if (lockExpireTime > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                return false;
            }

            //If the expire time is less than the current time then it wasn't released properly and we can attempt to 
            //acquire the lock. This is done by setting the lock to our timeout string AND checking to make sure
            //that what is returned is the old timeout string in order to account for a possible race condition.
            var oldString = redisWrapper.GetAndSet(key, lockString).ToString();
            return oldString == lockExpireString;
        }

        public void UnLock(string key)
        {
            redisWrapper.Remove(key);
        }
    }
}

