LonelyTank
==========

Танки с бесконечной картой. Игрок управляет танком. Вид строго сверху (2D).
Мир бесконечный, в нем есть четыре вида объектов: деревья, кусты, камни, лужи. Игрок может ехать сколь угодно долго, он не упрется в край мира.
Мир дискретный, то есть объекты помещаются в узлы сетки. Количество объектов на экране — 50, относительная вероятность появления объектов: деревья — 10%, кусты — 30%, лужи — 10%, камни — 50%.
Мир запоминается, то есть, если игрок в другой сессии попадает в ту же точку, он видит тот же пейзаж (то же расположение объектов).
Раз в 5 секунд на карте генерируется куст случайным образом (вероятность появления тем больше, чем меньше кустов рядом)
Управление танком: влево/вправо — поворот танка, вверх/вниз — ехать вперед/назад.
Есть возможность начать заново (этот уровень удаляется, стартуем сначала)
Мир создает клиент, мультиплейера нет, присоединения других игроков нет.


Вопрос:
Что меняется, если мы начинаем использовать сервер для игры? То есть, есть возможность присоединиться другим игрокам.