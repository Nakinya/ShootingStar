# ShootingStar
项目资源及演示视频：链接：https://pan.baidu.com/s/1laoKw1uTHsTvFJNWHIAb1Q 
提取码：zzfn
# 项目说明
此项目为一个横版射击游戏，项目结构包括UI系统、角色系统、战斗系统、能量系统、战利品系统、积分系统、关卡系统、数据保存系统，代码编写使用了对象池、单例模式等设计模式、规则
## UI系统
主菜单、暂停菜单、结束页面、积分榜菜单
## 角色系统
分为玩家和敌人两种，玩家拥有生命值，生命值随时间缓慢恢复，玩家的操作使用的是unity的Input System。
<br />敌人分为3种普通敌人和1个boss敌人，不同敌人有不同的武器，通过不同逻辑对玩家进行攻击
## 战斗系统
玩家的武器分为三种等级，分别发射不同密度的子弹，受到攻击时武器降级。玩家拥有副武器导弹，数量有限威力巨大，此外当玩家能量满时可以进入过载状态，发射大量追踪子弹，同时能量消耗巨大。玩家可以翻滚，翻滚时可躲避敌人子弹。
## 能量系统
玩家翻滚、过载都会消耗能量，射击击中敌人或者拾取道具可以补充能量
## 战利品系统
敌人死亡后会掉落战利品，类型包括增加分数道具、增加导弹道具、增加能量道具、升级武器道具、回复生命道具
## 积分系统
游戏时会自动累计分数，游戏结束后保存分数并对历史分数进行排名，排名情况在游戏结束后的积分榜公布
## 关卡系统
玩家消灭所有当前关卡敌人后会自动进入下一关卡，每五关为一个boss关，随着关卡数增加敌人将变得越来越强
## 数据保存系统
玩家的数据以json文本格式保存在本地，以进行积分排名
