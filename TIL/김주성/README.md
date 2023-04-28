# TIL

## 2023-04-13

- 특화 결선 발표
- Unity 개인 학습
- 오브젝트 생성 및 설정

## 2023-04-14

- 컨설턴트 님 정기 상담

## 2023-04-21

- 스프링 초기 환경 구축

## 2023-04-24

- 아이템, 인벤토리 기초 테스트

## 2023-04-26

- 아이템, 인벤토리, 오브젝트 상호 작용 기초 구현

## 2023-04-27

- 아이템, 인벤토리, 오브젝트, 자물쇠 상호 작용 기초 구현

## 2023-04-28

- 아이템, 인벤토리, 오브젝트, 자물쇠 상호 작용 구현
- 레이캐스팅을 통해 오브젝트와 상호작용 구현
```
// 상호작용 할 수 있는 오브젝트를 끊임없이 탐색하는 함수
    // 키보드, 마우스를 이리저리 움직일 때 계속 레이캐스트를 쏘며 상호작용 할 수 있는 오브젝트가 있는 지 확인한다.
    private void CheckInteractionObjects()
    {
        // 레이캐스트를 쏜 지점에 상호작용 오브젝트가 있는지 확인
        if (Physics.Raycast(
            transform.position, 
            transform.TransformDirection(Vector3.forward), 
            out hitInfo, 
            range, 
            (1 << 6) | (1 << 7) | (1 << 8))
        )
        {
            ClearItemHighlight();

            if (hitInfo.transform.tag == "Item")
            {
                ShowItemAcquireMessage();   // 아이템 획득 메시지 활성화
                ShowItemHighlight();        // 아이템 하이라이트 활성화
            }

            // 상호작용 객체에 대한 설명이 나오는 동안은 메시지, 하이라이트 비활성화
            if (hitInfo.transform.tag == "Interaction" && !InteractionObject.interactionMessageActivated)
            {
                ShowObjectInteractionMessage();
                ShowItemHighlight();
            }

            if (hitInfo.transform.tag == "Lock")
            {
                ShowLockInteractionMessage();
                ShowLockHighlight();
            }
        }
        else
        {
            if (preHitInfo.transform != null)
            {
                ItemInfoDisappear();
                InteractionInfoDisappear();
                ClearLockInteractionMessage();

                if (preHitInfo.transform.tag == "Item" || preHitInfo.transform.tag == "Interaction")
                {
                    ClearItemHighlight();
                }

                if (preHitInfo.transform.tag == "Lock")
                {
                    ClearHasChildObjectHighlight();
                }
            }
        }
    }
```