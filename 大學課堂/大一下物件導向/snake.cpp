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
public:
    Role(){
        name = "role";
        icon = 'R';
        x = 0;
        y = 0;
    }
    Role(string n, char i, int x, int y) : name(n), icon(i), x(x), y(y) {
    }

    int GetX(){ return this->x; };
    int GetY(){ return this->y; };
    void SetX(int x){ this->x = x; };
    void SetY(int y){ this->y = y; };

    string GetName(){ return name; };
    char GetIcon(){ return icon; };
};

class Snake : public Role{ // 蛇
private:
    char face; // 蛇的方向
    int rate; //蛇的速度
public:
    Snake() : Snake(0, 0, 'L') {
    }
    Snake(int x, int y, char f) : Role("snake", 'S', x, y) {
        face = f;
        rate = 1;
    }
    char GetFace(){ return face; };
    void SetFace(char f){ this->face = f; };
};

class Apple : public Role{ // 蘋果
public:
    Apple() : Role("apple", 'A', 0, 0) {
    }
    Apple(int x, int y) : Role("apple", 'A', x, y) {
    }
    void GotHit(){
        cout << name << ": got hit" << endl;
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
    Apple* apple;
    deque<Snake*> snakes;
    vector<vector<int>> v;
    COORD coord;
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    void SetCursorPosition(int x, int y) {
        coord.X = x;
        coord.Y = y;
        SetConsoleCursorPosition(hConsole, coord);
    }
    void HideCursor() {
        CONSOLE_CURSOR_INFO cursorInfo;
        GetConsoleCursorInfo(hConsole, &cursorInfo);
        cursorInfo.bVisible = false; // 隱藏游標
        SetConsoleCursorInfo(hConsole, &cursorInfo);
    }

    void Move(Role role, char face, int &ox, int &oy){ //根據蛇頭的方向改變座標不改變蛇頭
        int x = role.GetX();
        int y = role.GetY();
        switch(face){ //根據方向改變座標
            case 'U':
                if(y - 1 >= 0) y -= 1;
                break;
            case 'D':
                if(y + 1 < height) y += 1;
                break;
            case 'L':
                if(x - 1 >= 0) x -= 1;
                break;
            case 'R':
                if(x + 1 < width) x += 1;
                break;
        }
        ox = x; //傳回新的座標
        oy = y; //傳回新的座標
    }
public:
    Map() : Map(20, 20){
        // v = new vector<vector<int>>(height, vector<int>(width, 0));
        // apple = SetPos(new Apple(0, 0));
    }
    Map(int w, int h) : width(w), height(h) {
        HideCursor();
        v = vector<vector<int> >(height, vector<int>(width, 0));
        apple = new Apple();
        SetPos(*apple);
        snakes.push_back(new Snake());
        SetPos(*snakes[0]);
        // int tempx;
        // int tempy;
        // Move(*snakes[0], 'R', tempx, tempy); //測試用
        // snakes.push_back(new Snake(tempx, tempy)); //測試用
        // Move(*snakes[0], 'R', tempx, tempy); //測試用
        // snakes.push_back(new Snake(tempx, tempy)); //測試用
        SetCursorPosition(apple->GetX(), apple->GetY()); //設定蘋果位置
        cout << apple->GetIcon();
    }

    ~Map(){
        delete apple;
        for(auto temp : snakes){
            delete temp;
        }
        //在偵錯時聽取copilot的建議，將記憶體釋放
    }

    void SetPos(Role &role){
        int x = rand() % width;
        int y = rand() % height;
        while(v[y][x] != 0){
            x = rand() % width;
            y = rand() % height;
        }
        v[y][x] = 1;
        role.SetX(x);
        role.SetY(y);
    }

    void Control(char keycode){
        switch(keycode){
            case 72: // ↑
                snakes.front()->SetFace('U');
                break;
            case 80: // ↓
                snakes.front()->SetFace('D');
                break;
            case 75: // ←
                snakes.front()->SetFace('L');
                break;
            case 77: // →
                snakes.front()->SetFace('R');
                break;
            default:
                break;
        }
    }

    void Print(){ //印出地圖
        //system("cls");
        //SetCursorPosition(apple->GetX(), apple->GetY()); //設定蘋果位置
        // for(auto temp : snakes){
        //     SetCursorPosition(temp->GetX(), temp->GetY());
        //     cout << temp->GetIcon();
        // }
        SetCursorPosition(snakes.back()->GetX(), snakes.back()->GetY());
        cout << ' ';
        int tempx, tempy;
        char f;
        Move(*snakes.front(), snakes.front()->GetFace(), tempx, tempy); //測試用
        f = snakes.front()->GetFace(); //測試用
        snakes.push_front(new Snake(tempx, tempy, f));
        snakes.pop_back();
        SetCursorPosition(snakes.front()->GetX(), snakes.front()->GetY());
        cout << snakes.front()->GetIcon(); //印出蛇頭
    }
};

int main(){
    srand(time(NULL));
    Map* map = new Map(20, 20);
    while(true){
        if(kbhit()) {
            char ch = getch();
            if (ch == -32 || ch == 0) { // 方向鍵開頭碼
                char arrow = getch();   // 第二個字元
                map->Control(arrow);    // 傳進第二字元
            }
        }
        map->Print();
        Sleep(500);
    }
    cin.get();
    return 0;
}