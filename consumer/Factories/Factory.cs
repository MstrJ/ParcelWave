using consumer.Repositories;
using consumer.Services;

namespace consumer.Factory;

public static class Factory
{
    
    public static IParcelService CreateParcelService()
    {
        return new ParcelService(new ParcelRepository());
    }
    
    public static IKafkaProducerService CreateKafkaProducerService()
    {
        return new KafkaProducerService();
    }
    
}