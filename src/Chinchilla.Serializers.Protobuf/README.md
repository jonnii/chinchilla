chinchilla
==========

## Protobuf serialization


Protobuf serialization requires the registration of the generated classes with the registrar.
````

var serializers = new MessageSerializers();
var registrar = new ProtobufRegistrar();
registrar.Register(PriceMessage.Parser);
registrar.Register(ConnectMessage.Parser);
serializers.Default = new ProtobufMessageSerializer(registrar);

````


[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/jonnii/chinchilla/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

