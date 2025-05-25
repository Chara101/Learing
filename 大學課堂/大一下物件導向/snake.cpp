#include <bits/stdc++.h>
#include <windows.h>
#include <conio.h>
#include <chrono>
using namespace std;
using namespace chrono;

class CmdHandler{
private:
    COORD coord;
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
public:
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
};

class Coordinate{
private:
    int x;
    int y;
public:
    Coordinate() : x(0), y(0) {
    }
    Coordinate(int x, int y) : x(x), y(y) {
    }

    int GetX(){ return this->x; };
    int GetY(){ return this->y; };
    void SetX(int x){ this->x = x; };
    void SetY(int y){ this->y = y; };
};

class Role{ // 角色基底
protected:
    string name;
    char icon;
    int x;
    int y;
    int id;
public:
    Role() : Role("role", 'R', 0, 0, 0) {
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
public:
    Snake() : Snake(0, 0, 'L') {
    }
    Snake(int x, int y, char f) : Role("snake", 'S', x, y, 1) {
        face = f;
        rate = 500;
    }

    char GetFace(){ return face; };
    void SetFace(char f){ this->face = f; };
    int GetRate(){ return rate; };
    void SetRate(int r){ this->rate = r; };
};

class Apple : public Role{ // 蘋果
public:
    Apple() : Role("apple", 'A', 0, 0, 2) {
    }
    Apple(int x, int y) : Role("apple", 'A', x, y, 2) {
    }
};

class Snakes{
private:
    CmdHandler cmdh;
    deque<Snake*> snakes; //蛇的整個身體
    vector<vector<int>> v;
public:
    Snakes(int h, int w, int x, int y){
        v = vector<vector<int>>(20, vector<int>(20, 0)); //初始化地圖
        v[y][x] = 1; //紀錄蛇頭的位置
        snakes.push_back(new Snake(x, y, 'L')); //初始蛇頭
        snakes.push_back(new Snake(x, y, 'L')); //初始蛇頭
        snakes.push_back(new Snake(x, y, 'L')); //初始蛇頭
    }
    ~Snakes(){
        for(auto temp : snakes){
            delete temp;
        }
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

    int Move(int height, int width){ //根據蛇頭的方向改變座標不改變蛇頭
        int status = 0; //紀錄蛇的狀態 0:正常 1:撞牆 2:撞到自己 3:吃到蘋果
        int x = snakes[0]->GetX();
        int y = snakes[0]->GetY();
        switch(snakes[0]->GetFace()){ //根據方向改變座標
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
        if(status != 0) return status;
        if(v[y][x] == 1){ //撞到自己
            status = 2;
            return status;
        }
        v[y][x] = 1; //紀錄蛇頭的位置
        snakes.push_front(new Snake(x, y, snakes.front()->GetFace())); //新增蛇頭
        return status; //傳回狀態
    }

    void EatApple(){ //吃到蘋果
        int x = snakes.back()->GetX();
        int y = snakes.back()->GetY();
        char f = snakes.back()->GetFace();
        snakes.push_back(new Snake(x, y, f));
    }

    int Action(){
        int tempx, tempy;
        char f;
        int status = 0;
        status = Move(20, 20); //測試用
        f = snakes.front()->GetFace(); //測試用
        snakes.push_front(new Snake(tempx, tempy, f));
    }

    void Print(){ //印出地圖
        cmdh.SetCursorPosition(snakes.back()->GetX() + 1, snakes.back()->GetY() + 1);
        cout << ' '; //刪除蛇尾
        cmdh.SetCursorPosition(snakes.front()->GetX() + 1, snakes.front()->GetY() + 1);
        cout << snakes.front()->GetIcon(); //印出蛇頭
    }
    deque<Snake*>& GetSnakes(){ return snakes; }
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
    Snakes* snakes;//蛇的整個身體
    Apple* apple;
    CmdHandler cmdh; 
    vector<vector<int>> v; //紀錄地圖的使用狀況 0空 1蛇 2蘋果

    void PrintFrame(){ //印出邊框
        for(int i = 0; i < width + 2; i++){ //頂邊
            cmdh.SetCursorPosition(i, 0);
            cout << '#';
        }
        for(int i = 0; i < height + 2; i++){ //側邊
            cmdh.SetCursorPosition(0, i);
            cout << '#';
            cmdh.SetCursorPosition(width + 1, i);
            cout << '#';
        }
        cmdh.SetCursorPosition(0, height + 1);
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
    Map(int w, int h) : width(w), height(h), snakes(new Snakes(h, w, 0, 0)) {
        cmdh.HideCursor();
        v = vector<vector<int> >(height, vector<int>(width, 0));
        apple = new Apple();
        SetPos(*apple);
        cmdh.SetCursorPosition(apple->GetX() + 1, apple->GetY() + 1); //設定蘋果位置
        cout << apple->GetIcon();
        PrintFrame(); //印出邊框
    }

    ~Map(){
        delete apple;
        delete snakes;
    }

    void Print(){ //印出地圖
        
    }

    int GetGameOver(){ //遊戲結束的狀態 0:正常 1:遊戲結束
        return gameover;
    };
};

class SnakeGame{
private:
    Map* map;
    steady_clock::time_point start = steady_clock::now();
public:
    SnakeGame() : map(new Map(20, 20)) {
        srand(time(NULL));
        
    }
    ~SnakeGame(){
    }
    void Snake(){
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
            if(duration_cast<milliseconds>(steady_clock::now() - start).count() >= 500){
                map->Print();
                start = steady_clock::now(); // 重置時間
            }
            map->Action(); //執行動作
        }
    }
};

int main(){
    SnakeGame* game = new SnakeGame();
    game->Snake();
    delete game;
    cin.get();
    return 0;
}