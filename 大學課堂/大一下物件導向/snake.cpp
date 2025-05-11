#include <bits/stdc++.h>
#include <windows.h>
#include <conio.h>
using namespace std;

class Role{ // 角色基底
protected:
    string name;
    char icon;
    int x;
    int y;
    int id;
public:
    Role(){
        name = "role";
        icon = 'R';
        x = 0;
        y = 0;
        id = 0;
    }
    Role(string n, char i, int x, int y, int d) : name(n), icon(i), x(x), y(y), id(d) {
    }

    int GetX(){ return this->x; };
    int GetY(){ return this->y; };
    int GetId(){ return this->id; };
    void SetX(int x){ this->x = x; };
    void SetY(int y){ this->y = y; };

    string GetName(){ return name; };
    char GetIcon(){ return icon; };
};

class Snake : public Role{ // 蛇
private:
    char face; // 蛇的方向
    int rate; //蛇的速度
    int id = 1;
public:
    Snake() : Snake(0, 0, 'L') {
    }
    Snake(int x, int y, char f) : Role("snake", 'S', x, y, 1) {
        face = f;
        rate = 1;
    }
    char GetFace(){ return face; };
    void SetFace(char f){ this->face = f; };
};

class Apple : public Role{ // 蘋果
public:
    Apple() : Role("apple", 'A', 0, 0, 2) {
    }
    Apple(int x, int y) : Role("apple", 'A', x, y, 2) {
    }
    void GotHit(){
        // cout << name << ": got hit" << endl;
    }
};

class Map{
    /*
        v陣列紀錄地圖的使用狀況
        snakes紀錄蛇的整個身體
    */
private:
    int width;
    int height;
    int gameover = 0; //遊戲結束的狀態 0:正常 1:遊戲結束
    Apple* apple;
    deque<Snake*> snakes;
    vector<vector<int>> v; //紀錄地圖的使用狀況 0空 1蛇 2蘋果
    COORD coord;
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    void SetCursorPosition(int x, int y) {
        coord.X = x;
        coord.Y = y; // +1是因為邊框的關係
        SetConsoleCursorPosition(hConsole, coord);
    }
    void HideCursor() {
        CONSOLE_CURSOR_INFO cursorInfo;
        GetConsoleCursorInfo(hConsole, &cursorInfo);
        cursorInfo.bVisible = false; // 隱藏游標
        SetConsoleCursorInfo(hConsole, &cursorInfo);
    }

    int Move(Snake role, char face, int &ox, int &oy){ //根據蛇頭的方向改變座標不改變蛇頭
        int status = 0; //紀錄蛇的狀態 0:正常 1:撞牆 2:撞到自己 3:吃到蘋果
        int x = role.GetX();
        int y = role.GetY();
        switch(face){ //根據方向改變座標
            case 'U':
                if(y - 1 >= 0) y -= 1;
                else status = 1; //撞牆
                break;
            case 'D':
                if(y + 1 < height) y += 1;
                else status = 1; //撞牆
                break;
            case 'L':
                if(x - 1 >= 0) x -= 1;
                else status = 1; //撞牆
                break;
            case 'R':
                if(x + 1 < width) x += 1;
                else status = 1; //撞牆
                break;
        }
        if(v[y][x] == 1){ //撞到自己
            status = 2;
        }
        else if(v[y][x] == 2){ //吃到蘋果
            status = 3;
            apple->GotHit(); //吃到蘋果
            v[y][x] = 0; //將蘋果的座標設為空
            SetPos(*apple); //重新設定蘋果的座標
            SetCursorPosition(apple->GetX() + 1, apple->GetY() + 1); //設定蘋果位置
            cout << apple->GetIcon(); //印出蘋果
        }
        v[y][x] = role.GetId(); //紀錄地圖的使用狀況
        ox = x; //傳回新的座標
        oy = y; //傳回新的座標
        return status; //傳回狀態
    }

    void PrintFrame(){ //印出邊框
        for(int i = 0; i < width + 2; i++){ //頂邊
            SetCursorPosition(i, 0);
            cout << '#';
        }
        for(int i = 0; i < height + 2; i++){ //側邊
            SetCursorPosition(0, i);
            cout << '#';
            SetCursorPosition(width + 1, i);
            cout << '#';
        }
        SetCursorPosition(0, height + 1);
        for(int i = 0; i < width + 2; i++){
            cout << '#';
        }

    }
    void SetPos(Role &role){
        int x = rand() % width;
        int y = rand() % height;
        while(v[y][x] != 0){
            x = rand() % width;
            y = rand() % height;
        }
        v[y][x] = role.GetId(); //紀錄地圖的使用狀況
        role.SetX(x);
        role.SetY(y);
    }
public:
    Map() : Map(20, 20){
    }
    Map(int w, int h) : width(w), height(h) {
        HideCursor();
        v = vector<vector<int> >(height, vector<int>(width, 0));
        apple = new Apple();
        SetPos(*apple);
        snakes.push_back(new Snake());
        SetPos(*snakes[0]);
        snakes.push_back(new Snake(*snakes[0]));
        snakes.push_back(new Snake(*snakes[0]));
        SetCursorPosition(apple->GetX() + 1, apple->GetY() + 1); //設定蘋果位置
        cout << apple->GetIcon();
        PrintFrame(); //印出邊框
    }

    ~Map(){
        delete apple;
        for(auto temp : snakes){
            delete temp;
        }
        //在偵錯時聽取copilot的建議，將記憶體釋放
    }

    void Control(char keycode){
        switch(keycode){
            case 72: //上
                snakes.front()->SetFace('U');
                break;
            case 80: //下
                snakes.front()->SetFace('D');
                break;
            case 75: //左
                snakes.front()->SetFace('L');
                break;
            case 77: //右
                snakes.front()->SetFace('R');
                break;
            default:
                break;
        }
    }

    void Print(){ //印出地圖
        Sleep(500);
        //system("cls");
        //SetCursorPosition(apple->GetX(), apple->GetY()); //設定蘋果位置
        // for(auto temp : snakes){
        //     SetCursorPosition(temp->GetX(), temp->GetY());
        //     cout << temp->GetIcon();
        // }
        SetCursorPosition(snakes.back()->GetX() + 1, snakes.back()->GetY() + 1);
        cout << ' ';
        int tempx, tempy;
        char f;
        int status = 3;
        status = Move(*snakes.front(), snakes.front()->GetFace(), tempx, tempy); //測試用
        f = snakes.front()->GetFace(); //測試用
        snakes.push_front(new Snake(tempx, tempy, f));
        if(status != 3) snakes.pop_back();
        if(status == 1 || status == 2){ //撞牆或撞到自己
            gameover = 1; //遊戲結束
        }
        SetCursorPosition(snakes.front()->GetX() + 1, snakes.front()->GetY() + 1);
        cout << snakes.front()->GetIcon(); //印出蛇頭
    }

    int GetGameOver(){ //遊戲結束的狀態 0:正常 1:遊戲結束
        return gameover;
    };
};

int main(){
    srand(time(NULL));
    Map* map = new Map(20, 20);
    while(true){
        if(map->GetGameOver() == 1){ //遊戲結束
            system("cls");
            cout << "Game Over" << endl;
            break;
        }
        if(kbhit()) {
            char ch = getch();
            if (ch == -32 || ch == 0) { // 方向鍵開頭碼
                char arrow = getch();   // 第二個字元
                map->Control(arrow);    // 傳進第二字元
            }
        }
        map->Print();
    }
    cin.get();
    return 0;
}