#include <bits/stdc++.h>
#include <windows.h>
#include <conio.h>
#include <chrono>
using namespace std;
using namespace chrono;

enum class SnakeisEaten{
    Yes,
    No
};

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

class IRole{
public:
    virtual Coordinate* GetCoor() = 0;
    virtual int GetId() = 0;
    virtual string GetName() = 0;
    virtual char GetIcon() = 0;
    virtual void SetCoor(Coordinate* c) = 0;
};

class Role : public IRole{ // 角色基底
protected:
    string name;
    char icon;
    Coordinate* coor; //座標
    int id;
public:
    Role() : Role("role", 'R', 0, 0, 0) {
    }
    Role(string n, char i, int x, int y, int d) : name(n), icon(i), coor(new Coordinate(x, y)), id(d) {
    }
    Role(string n, char i, Coordinate* c, int d) : name(n), icon(i), coor(c), id(d) {
    }
    ~Role(){
        delete coor; // 釋放座標記憶體
    }
    Coordinate* GetCoor(){ return coor; };
    void SetCoor(Coordinate* c){ this->coor = c; }
    int GetId(){ return this->id; };

    string GetName(){ return name; };
    char GetIcon(){ return icon; };
};

// class Snake : public Role{ // 蛇
// private:
//     char face; // 蛇的方向
//     int rate; //蛇的速度
// public:
//     Snake() : Snake(0, 0, 'L', 500) {
//     }
//     Snake(int x, int y, char f, int r) : Role("snake", 'S', x, y, 1), rate(r) {
//         face = f;
//     }
//     Snake(Coordinate* c, char f, int r):Role("snake", 'S', c, 1), rate(r) {
//         face = f;
//     }

//     char GetFace(){ return face; };
//     void SetFace(char f){ this->face = f; };
//     int GetRate(){ return rate; };
//     void SetRate(int r){ this->rate = r; };
// };

// class Apple : public Role{ // 蘋果
// public:
//     Apple() : Role("apple", 'A', 0, 0, 2) {
//     }
//     Apple(int x, int y) : Role("apple", 'A', x, y, 2) {
//     }
//     Apple(Coordinate* c) : Role("apple", 'A', c, 2) {
//     }
// };

class Map{
private:
    int width;
    int height;
    int gameover = 0; //遊戲結束的狀態 0:正常 1:遊戲結束
    int status = 0; //狀態: 0:正常 1:遊戲結束 2:初始化失敗
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
public:
    Map() : Map(20, 20){
    }
    Map(int w, int h) : width(w), height(h) {
        cmdh.HideCursor();
        v = vector<vector<int> >(height, vector<int>(width, 0));
        PrintFrame(); //印出邊框
    }

    ~Map(){
    }

    Coordinate* GetSpace(){
        int x = rand() % width;
        int y = rand() % height;
        while(v[y][x] != 0){
            x = rand() % width;
            y = rand() % height;
        }
        return new Coordinate(x, y);
    }

    int CheckSpace(Coordinate* c){ //檢查座標是否可以使用
        if(c == nullptr) return -1;
        if(c->GetX() < 0 || c->GetX() >= width || c->GetY() < 0 || c->GetY() >= height) return -1; //超出邊界
        return v[c->GetY()][c->GetX()];
    }

    int CheckSpace(int& x, int& y){ //檢查座標是否可以使用
        if(x < 0 || x >= width || y < 0 || y >= height) return -1; //超出邊界
        return v[y][x];
    }

    void Mark(Coordinate* c, int id){ //標記地圖上的位置
        v[c->GetY()][c->GetX()] = id;
    }

    void Mark(int& x, int& y, int id){ //標記地圖上的位置
        v[y][x] = id;
    }

    void Unmark(Coordinate* c){ //取消標記地圖上的位置
        v[c->GetY()][c->GetX()] = 0;
    }

    void Unmark(int& x, int& y){ //取消標記地圖上的位置
        v[y][x] = 0;
    }

    int GetGameOver(){ //遊戲結束的狀態 0:正常 1:遊戲結束
        return gameover;
    };
    void SetGameOver(int g){ //設定遊戲結束的狀態
        gameover = g;
    };
    int GetWidth(){ return width; };
    int GetHeight(){ return height; };
};

class Snakes{
private:
    CmdHandler _cmdh;
    deque<IRole*> _snakes; //蛇的整個身體
    char _face;
    int _rate;
    int _status = 0; //0: common, 1:dead
    const int _id = 1;
public:
    Snakes(vector<Coordinate*> temp) : _face('L'), _rate(500){
        for(Coordinate* c : temp){
            _snakes.push_back(new Role("snake", 'S', c, _id));
        }
    }
    ~Snakes(){
        for(auto temp : _snakes){
            delete temp;
        }
    }

    void Turn(char keycode){
        switch(keycode){
            case 72: //上
                _face = 'U';
                break;
            case 80: //下
                _face = 'D';
                break;
            case 75: //左
                _face = 'L';
                break;
            case 77: //右
                _face = 'R';
                break;
            default:
                break;
        }
    }

    void Hasten(int command){
        if(command == 43){ //加速
            if(_rate >= 400) _rate -= 50; //最小速度100ms
        } else if(command == 45){ //減速
            if(_rate <= 600) _rate += 50; //最大速度500ms
        }
    }

    void Move(Coordinate* c, SnakeisEaten e){
        _snakes.push_front(new Role("snake", 'S', c, _id)); 
        if(e != SnakeisEaten::Yes) _snakes.pop_back();
    }

    deque<IRole*>& GetSnakes(){ return _snakes; }
    int GetStatus(){ return _status; } //取得蛇的狀態
    int GetRate(){ return _rate; } //取得蛇的速度
};

class Apples{
private:
    vector<IRole*> _apples; //蘋果的整個身體
    const int _id = 2;
public:
    Apples(vector<Coordinate*> temp) {
        for(Coordinate* c : temp){
            _apples.push_back(new Role("apple", 'A', c, _id)); //新增蘋果
        }
    }
    ~Apples(){
        for(auto apple : _apples){
            delete apple;
        }
    }

    void AddApple(Coordinate* c){
        _apples.push_back(new Role("apple", 'A', c, _id));
    }

    void GotHit(Coordinate* c){ //被蛇吃掉
        auto tar = find_if(_apples.begin(), _apples.end(), [c](auto x){ return x->GetCoor()->GetX() == c->GetX() && x->GetCoor()->GetY() == c->GetY(); });
        if(tar != _apples.end()){
            delete *tar;
            _apples.erase(tar);
        }
    }

    vector<IRole*>& GetApples(){ return _apples; }
};

class SnakeGame{
private:
    int score = 0;
    int pace = 3;
    int level = 1;
    Map* map1;
    Snakes* snakes;
    Apples* apples;
    CmdHandler cmdh;
    steady_clock::time_point pstart = steady_clock::now();
    steady_clock::time_point Snstart = steady_clock::now();
public:
    SnakeGame() : map1(new Map(20, 20)) {
        srand(time(NULL));
        snakes = new Snakes(map1);
        apples = new Apples(map1, 3); //初始蘋果
    }
    ~SnakeGame(){
        delete map1;
        delete snakes;
        delete apples;
    }

    void Print(){
        for(auto apple : apples){
            cmdh.SetCursorPosition(apple->GetCoor()->GetX() + 1, apple->GetCoor()->GetY() + 1);
            cout << apple->GetIcon();
        }
    }

    int Move(){ //移動蛇
        int status = 0; //紀錄蛇的狀態 0:正常 1:撞牆 2:撞到自己 3:吃到蘋果
        int height = map1->GetHeight();
        int width = map1->GetWidth();
        int x = snakes[0]->GetCoor()->GetX();
        int y = snakes[0]->GetCoor()->GetY();
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
        if(map1->CheckSpace(x, y) == 1){ //撞到自己
            status = 2;
            return status;
        }
        if(map1->CheckSpace(x, y) == 2){
            status = 3;
        }
        snakes.push_front(new Snake(x, y, snakes.front()->GetFace(), snakes.front()->GetRate())); //新增蛇頭
        map1->Mark(snakes.front()->GetCoor(), snakes.front()->GetId()); //紀錄蛇頭的位置
        if(status != 3){
            map1->Unmark(snakes.back()->GetCoor()); //取消蛇尾的位置
            delete snakes.back(); //釋放蛇尾的記憶體
            snakes.pop_back(); //移除蛇尾
        }
        return status; //傳回狀態
    }

    void Print(){ //印出
        cmdh.SetCursorPosition(snakes.front()->GetCoor()->GetX() + 1, snakes.front()->GetCoor()->GetY() + 1);
        cout << snakes.front()->GetIcon();
        cmdh.SetCursorPosition(snakes.back()->GetCoor()->GetX() + 1, snakes.back()->GetCoor()->GetY() + 1);
        cout << ' ';
    }

    void Control(){
        int ch = getch();
        if (ch == 224 || ch == 0) { // 方向鍵開頭碼
            char arrow = getch();   // 第二個字元
            snakes->Turn(arrow);    // 傳進第二字元
            return;
        }
        if(ch == 43 || ch == 45){
            snakes->Hasten(ch); //加速或減速
            if(ch == 43) {
                if(pace < 5) pace++;
            } else if(ch == 45) {
                if(pace > 1) pace--;
            }
            return;
        }
    }

    void Print(){
        cmdh.SetCursorPosition(25, 23);
        cout << "Time:" << duration_cast<seconds>(steady_clock::now() - pstart).count() << "s";
        cmdh.SetCursorPosition(25, 14);
        cout << "Score: " << score;
        cmdh.SetCursorPosition(25, 15);
        cout << "Pace: " << pace;
        cmdh.SetCursorPosition(25, 16);
        cout << "Level: " << level;
    }

    void Snake(){
        while(true){
            if(map1->GetGameOver() == 1){ //遊戲結束
                system("cls");
                cout << "Game Over" << endl;
                break;
            }
            if(kbhit()) {
                Control();
            }
            if(level < 5){
                level = (score / 10) + 1; //每10分升一級
            }
            steady_clock::time_point now = steady_clock::now();
            if(duration_cast<milliseconds>(steady_clock::now() - Snstart).count() >= (snakes->GetRate() - (25 * (level - 1)))){
                Coordinate* c = new Coordinate();
                int result = snakes->Move();
                snakes->Print(); //印出蛇
                if(result == 3){
                    apples->GotHit(snakes->GetSnakes().front()->GetCoor()); //蘋果被吃掉
                    score++;
                }
                if(result == 1 || result == 2){ //撞牆或撞到自己
                    map1->SetGameOver(1); //設定遊戲結束
                }
                Snstart = steady_clock::now(); // 重置時間
                apples->Print(); //印出蘋果
                Print();
            }
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