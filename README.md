# 3DFlappyBird

json파일 충돌 때문에 커밋하기 전에 초기화 버튼 눌러주세요

설정

Edit - Project Settings - Editor - Force Text

Edit - Project Settings - Version Control - Visible Meta Files

Bulid Setting에서 Android로 Switch

파일탐색기에서 파일을 옮기는 경우 메타파일이 재생성되서 참조가 사라집니다.
프로젝트 뷰에서만 파일을 수정해주세요.

메타파일은 프로젝트나 씬을 저장할 때 갱신됩니다. 유니티가 강제 종료되는 경우 메타파일 갱신이 안될 수 있습니다.

변수명 작성 방법
클래스나 메소드명은 파스칼 표기법을 따른다.(모든 단어에서 첫 문자는 대문자 나머지는 소문자)
ex) HelloWordl, NameViva

변수, 파라미터 등은 카멜 표기법을 따른다.
ex) helloWordl, nameViva

메서드 이름은 동사/전치사로 시작한다.
ex) countNumber, withUserId

상수는 대문자로 작성하고 복합어인 경우 '_'를 사용하여 단어를 구분한다.
ex) public final int SPECIAL_NUMBER = 1;

for문이나 if문 아래 코드 한 줄이여도 중괄호 무조건 사용

빌드할 때

Project Settings - Player - Resolution and Presentation - Allowed Orientation for Auto Rotaion - Portrait만 체크


