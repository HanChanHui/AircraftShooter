using Consts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class ParameterConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BaseParameters);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        ShootingType shootingType = (ShootingType)jo["shootingType"].Value<int>();

        BaseParameters baseParameters;

        switch (shootingType)
        {
            case ShootingType.Forward:
                baseParameters = new ForwardParameters();
                break;
            case ShootingType.Nway:
                baseParameters = new NwayParameters();
                break;
            case ShootingType.Circle:
                baseParameters = new NwayParameters();
                break;
            case ShootingType.RandomNway:
                baseParameters = new ForwardParameters();
                break;
            case ShootingType.Multiple:
                baseParameters = new MultipleParameters();
                break;
            case ShootingType.CustomShape:
                baseParameters = new CustomShapeParameters();
                break;
            case ShootingType.Homing:
                baseParameters = new HomingParameters();
                break;
            case ShootingType.DelayHoming:
                baseParameters = new DelayHomingParameters();
                break;
            case ShootingType.RandomHoming:
                baseParameters = new DelayHomingParameters();
                break;
            case ShootingType.RollingNway:
                baseParameters = new RollingNwayParameters();
                break;
            case ShootingType.WavingNway:
                baseParameters = new WavingNwayParameters();
                break;
            case ShootingType.CircleWavingNway:
                baseParameters = new WavingNwayParameters();
                break;
            case ShootingType.Placed:
                baseParameters = new PlacedParameters();
                break;
            case ShootingType.Aiming:
                baseParameters = new AimingParameters();
                break;
            case ShootingType.Spreading:
                baseParameters = new SpreadingParameters();
                break;
            case ShootingType.RandomSpreading:
                baseParameters = new RandomSpreadingParameters();
                break;
            case ShootingType.Overtaking:
                baseParameters = new OvertakingParameters();
                break;
            case ShootingType.Arc:
                baseParameters = new ArcParameters();
                break;
            case ShootingType.CustomShapeForward:
                baseParameters = new CustomShapeForwardParameters();
                break;
            default:
                throw new Exception("Unknown shooting type");
        }

        serializer.Populate(jo.CreateReader(), baseParameters);
        return baseParameters;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        JObject jo = new JObject();
        JToken t = JToken.FromObject(value);
        jo.Merge(t);
        jo.AddFirst(new JProperty("shootingType", (int)((BaseParameters)value).shootingType));
        jo.WriteTo(writer);
    }
}