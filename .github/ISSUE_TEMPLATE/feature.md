---
name: Feature request
about: New feature or update for existing
title: "feature(part) "
labels: ["type: feature"]
---

### 목표
- 무엇을 추가/개선하나요?


<br>

## 요구사항(간단히)
- 
- 

<br>

### 완료 기준(체크 가능하게)
- [ ] 
- [ ] 

---
name: "✨ Feature request"
description: "New feature or update for existing"
labels: ["feature"]
body:
  - type: textarea
    attributes:
      label: 📄 설명
      description: 새로운 기능에 대한 설명을 작성해 주세요.
      placeholder: 자세히 적을수록 좋습니다!
    validations:
      required: true
  - type: textarea
    attributes:
      label: ✅ 작업할 내용
      description: 할 일을 체크박스 형태로 작성해주세요.
      placeholder: 최대한 세분화 해서 적어주세요!
    validations:
      required: true
  - type: textarea
    attributes:
      label: 🙋🏻 참고 자료
      description: 참고 자료가 있다면 작성해 주세요.