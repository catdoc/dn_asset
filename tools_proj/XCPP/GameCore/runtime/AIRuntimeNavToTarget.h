/*
* <auto-generated>
*	 The code is generated by tools
*	 Edit manually this code will lead to incorrect behavior
* </auto-generated>
*/

#ifndef  __AIRuntimeNavToTarget__
#define  __AIRuntimeNavToTarget__

#include "../GameObject.h"
#include "../Vector3.h"
#include "../AIBehaviour.h"
#include "../AITreeImpleted.h"

class XEntity;

class AIRuntimeNavToTarget :public AIBase
{
public:
	~AIRuntimeNavToTarget();
	virtual void Init(AITaskData* data);
	virtual AIStatus OnTick(XEntity* entity);
	

private:
	GameObject* GameObjectmAIArgTarget;
	GameObject* GameObjectmAIArgNavTarget;
	Vector3 Vector3mAIArgNavPos;
	
};

#endif