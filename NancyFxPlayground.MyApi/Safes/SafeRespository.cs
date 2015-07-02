using System;

namespace NancyFxPlayground.MyApi.Safes
{
    public class SafeRespository : ISafeRespository
    {
        public Safe Get(int safeId)
        {
            return new Safe
            {
                Id = safeId,
                Name = "Safe " + safeId
            };
        }
    }
}