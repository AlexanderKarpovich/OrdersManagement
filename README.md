# OrdersManagement
Система для управления заказами промышленного предприятия.

## Domain
[Domain](https://github.com/AlexanderKarpovich/OrdersManagement/tree/master/Ordering.Domain) слой инкапсулирует в себе сущности предметной области и их поведение. Предметная область представлена тремя моделями:

- Order - представляет собой заказ
- OrderItem - элемент заказа, является внутренней сущностью заказа
- Provider - поставщик

Order содержит в себе логику, соответствующую требованиям предметной области. Provider является предопределённым множеством. Слой реализован в качестве библиотеки классов.

## Infrastructure
За слой инфраструктуры отвечает [Infrastructure](https://github.com/AlexanderKarpovich/OrdersManagement/tree/master/Ordering.Infrastructure). Инфраструктура содержит логику работы с данными: правила создания контекста базы данных, конфигурации сущностей, поддержание идемпотентности посылаемых клиентом запросов, а также реализует промежуточный слой взаимодействия с данным (репозитории). Слой реализован в виде библиотеки классов.

## API
[API](https://github.com/AlexanderKarpovich/OrdersManagement/tree/master/Ordering.API) является слоем приложения, содержащим логику взаимодействия различных сервисов, запросов и комманд, а также слоем представления, так как сохраняет состояние и отправляет данные о нём веб-клиенту. Работает на основе ASP.NET Web Api. В слой представления также входит **webclient**, работающий на основе **React**, поэтому **API** поддерживает политику **CORS**. 

### Начало работы с API
Перейдите в директорию с файлами **API**:

```sh
cd <PATH_TO_SOLUTION>\Ordering.API
```

где `<PATH_TO_SOLUTION>` представляет собой путь к файлу решения (`OrdersManagment.sln`).

После этого для запуска **API** выполните команду:

```sh
dotnet run
```

### Доступ к API
Доступ к **API** можно получить по следующим URL:
```sh
Orders Controller:   https://localhost:5001/api/orders
Providers Controller:   https://localhost:5001/api/providers
```

## Web Client
[Web Client](https://github.com/AlexanderKarpovich/OrdersManagement/tree/master/webclient) представляет собой UI, получающий данные от **API** и обрабатывающий их посредством **TypeScript** и **React**.

### Начало работы
Перейдите в директорию с файлами веб-клиента:

```sh
cd <PATH_TO_SOLUTION>\webclient
```

где `<PATH_TO_SOLUTION>` представляет собой путь к файлу решения (`OrdersManagment.sln`).

После этого для запуска API выполните команду:

```sh
npm start
```

### Доступ к webclient
Доступ к **webclient** можно получить по следующей URL:
```sh
https://localhost:3000
```

## License
MIT
