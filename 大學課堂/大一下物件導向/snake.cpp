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

enum class Direction{
    Up = 72,
    Down = 80,
    Left = 75,
    Right = 77
};

enum class GameStatus{
    Common,
    GameOver
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
    int status = 0; //狀態: 0:正常 1:遊戲結束 2:初始化失敗
    vector<vector<int>> v; //紀錄地圖的使用狀況 0空 1蛇 2蘋果
public:
    Map() : Map(20, 20){
    }
    Map(int w, int h) : width(w), height(h) {
        v = vector<vector<int> >(height, vector<int>(width, 0));
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

    Coordinate* GetAdjacentSpace(Coordinate* c, Direction f){ //取得座標的相鄰空位
        if(c == nullptr) return nullptr;
        int x = c->GetX();
        int y = c->GetY();
        switch(f){
            case Direction::Up:
                if(y - 1 >= 0) y -= 1;
                else return nullptr; //超出邊界
                break;
            case Direction::Down:
                if(y + 1 < height) y += 1;
                else return nullptr; //超出邊界
                break;
            case Direction::Left:
                if(x - 1 >= 0) x -= 1;
                else return nullptr; //超出邊界
                break;
            case Direction::Right:
                if(x + 1 < width) x += 1;
                else return nullptr; //超出邊界
                break;
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
    int GetWidth(){ return width; };
    int GetHeight(){ return height; };
};

class Snakes{
private:
    CmdHandler _cmdh;
    deque<IRole*> _snakes; //蛇的整個身體
    Direction _face;
    int _status = 0; //0: common, 1:dead
    const int _id = 1;
public:
    Snakes(vector<Coordinate*> temp) : _face(Direction::Left){
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
                _face = Direction::Up;
                break;
            case 80: //下
                _face = Direction::Down;
                break;
            case 75: //左
                _face = Direction::Left;
                break;
            case 77: //右
                _face = Direction::Right;
                break;
            default:
                break;
        }
    }

    void Move(Coordinate* c, SnakeisEaten e){
        _snakes.push_front(new Role("snake", 'S', c, _id)); 
        if(e != SnakeisEaten::Yes) _snakes.pop_back();
    }

    deque<IRole*>& GetSnakes(){ return _snakes; }
    int GetStatus(){ return _status; } //取得蛇的狀態
    Direction GetFace(){ return _face;}
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

class Board{
private:
    int _score = 0;
    int _bpace = 3;
    int _pace = 3;
    int _level = 1;
    steady_clock::time_point _start = steady_clock::now();
public:
    void operator++(){
        _score++;
        if(_score % 10 == 0) { //每10分升一級
            _level++;
            _bpace++;
            _pace++;
        }
    }

    void AddPace(){
        if(_pace < (_bpace + 2)) _pace++;
    }

    void ReducePace(){
        if(_pace > (_bpace - 2)) _pace--;
    }

    void ResetTime(){
        _start = steady_clock::now();
    }

    int GetScore(){ return _score;}
    int GetPace(){ return _pace; }
    int GetLevel(){ return _level; }
    steady_clock::time_point GetStartTime(){ return _start; }
    int GetRate(){ //取得速度
        int rate = 700 - (50 * _pace);
        if(rate < 100) rate = 100;
        return rate;
    }
    void SetScore(int score){ _score = score; }
    void SetPace(int pace){ _pace = pace; }
    void SetLevel(int level){ _level = level; }
};

class GameView {
private:
    CmdHandler _cmdh;

public:
    void PrintSnake(Snakes* s, SnakeisEaten e) {
        IRole* head = s->GetSnakes().front();
        IRole* end = s->GetSnakes().back();
        _cmdh.SetCursorPosition(head->GetCoor()->GetX() + 1, head->GetCoor()->GetY() + 1);
        cout << head->GetIcon();
        if(e == SnakeisEaten::Yes) return;
        _cmdh.SetCursorPosition(end->GetCoor()->GetX() + 1, end->GetCoor()->GetY() + 1);
        cout << ' ';
    }

    void PrintApples(Apples* apples) {
        for (IRole* apple : apples->GetApples()) {
            _cmdh.SetCursorPosition(apple->GetCoor()->GetX() + 1, apple->GetCoor()->GetY() + 1);
            cout << apple->GetIcon();
        }
    }

    void PrintBoard(Board* board) {
        _cmdh.SetCursorPosition(25, 23);
        cout << "Time: " << duration_cast<seconds>(steady_clock::now() - board->GetStartTime()).count() << "s";
        _cmdh.SetCursorPosition(25, 14);
        cout << "Score: " << board->GetScore();
        _cmdh.SetCursorPosition(25, 15);
        cout << "Pace: " << board->GetPace();
        _cmdh.SetCursorPosition(25, 16);
        cout << "Level: " << board->GetLevel();
    }

    void PrintFrame(Map* map1){ //印出邊框
        for(int i = 0; i < map1->GetWidth() + 2; i++){ //頂邊
            _cmdh.SetCursorPosition(i, 0);
            cout << '#';
        }
        for(int i = 0; i < map1->GetHeight() + 2; i++){ //側邊
            _cmdh.SetCursorPosition(0, i);
            cout << '#';
            _cmdh.SetCursorPosition(map1->GetWidth() + 1, i);
            cout << '#';
        }
        _cmdh.SetCursorPosition(0, map1->GetHeight() + 1);
        for(int i = 0; i < map1->GetWidth() + 2; i++){
            cout << '#';
        }

    }

    void ShowGameOver() {
        system("cls");
        cout << "Game Over" << endl;
    }
};


class SnakeGame{
private:
    
    Map* _map1;
    Snakes* _snakes;
    Apples* _apples;
    Board* _board;
    GameView _view;
    CmdHandler _cmdh;
    GameStatus _status = GameStatus::Common;
    SnakeisEaten _eaten = SnakeisEaten::No;
    steady_clock::time_point Snstart = steady_clock::now();

    void Initialize(){
        srand(time(NULL));
        _cmdh.HideCursor(); //隱藏游標
        _view.PrintFrame(_map1); //印出邊框
        _view.PrintBoard(_board); //印出分數板
        vector<Coordinate*> scoor(3);
        scoor[0] = _map1->GetSpace(); //蛇頭
        scoor[1] = _map1->GetAdjacentSpace(scoor[0], Direction::Left); //蛇身
        scoor[2] = _map1->GetAdjacentSpace(scoor[1], Direction::Left); //蛇尾
        _snakes = new Snakes(scoor);
        for(int i = 0; i < 3; i++){
            scoor[i] = _map1->GetSpace(); //取得空位
        }
        _apples = new Apples(scoor); //初始蘋果
        _view.PrintSnake(_snakes, _eaten); //印出蛇
        _view.PrintApples(_apples); //印出蘋果
    }
public:
    SnakeGame() : _board(new Board()), _map1(new Map(20, 20)) {
        Initialize();
    }
    ~SnakeGame(){
        delete _map1;
        delete _snakes;
        delete _apples;
    }

    Direction Reverse(Direction d){
        switch(d){
            case Direction::Up: return Direction::Down;
            case Direction::Down: return Direction::Up;
            case Direction::Left: return Direction::Right;
            case Direction::Right: return Direction::Left;
        }
    }

    pair<Direction, Direction> LDDirection(Direction d){
        switch(d){
            case Direction::Up: return make_pair(Direction::Left, Direction::Right);
            case Direction::Down: return make_pair(Direction::Right, Direction::Left);
            case Direction::Left: return make_pair(Direction::Down, Direction::Up);
            case Direction::Right: return make_pair(Direction::Up, Direction::Down);
        }
    }

    void Move(){ //移動蛇
        int height = _map1->GetHeight();
        int width = _map1->GetWidth();
        int x = _snakes->GetSnakes().front()->GetCoor()->GetX();
        int y = _snakes->GetSnakes().front()->GetCoor()->GetY();
        switch(_snakes->GetFace()){ //根據方向改變座標
            case Direction::Up:
                if(y - 1 >= 0) y -= 1;
                else _status = GameStatus::GameOver; //撞牆
                break;
            case Direction::Down:
                if(y + 1 < height) y += 1;
                else _status = GameStatus::GameOver; //撞牆
                break;
            case Direction::Left:
                if(x - 1 >= 0) x -= 1;
                else _status = GameStatus::GameOver; //撞牆
                break;
            case Direction::Right:
                if(x + 1 < width) x += 1;
                else _status = GameStatus::GameOver; //撞牆
                break;
        }
        if(_status != GameStatus::Common) return;
        Coordinate* temp = new Coordinate(x, y);
        if(_map1->CheckSpace(temp) == 1){ //撞到自己
            _status = GameStatus::GameOver;
            return;
        }
        if(_map1->CheckSpace(temp) == 2) _eaten = SnakeisEaten::Yes; //吃到蘋果
        else _eaten = SnakeisEaten::No; //沒有吃到蘋果
        _snakes->Move(temp, _eaten); //新增蛇頭
        _map1->Mark(temp, _snakes->GetSnakes().front()->GetId()); //紀錄蛇頭的位置
        if(_eaten == SnakeisEaten::No){
            _map1->Unmark(_snakes->GetSnakes().back()->GetCoor()); //取消蛇尾的位置
        }
    }

    

    void Control(){
        int ch = getch();
        if (ch == 224 || ch == 0) { // 方向鍵開頭碼
            char arrow = getch();   // 第二個字元
            _snakes->Turn(arrow);    // 傳進第二字元
            return;
        }
        if(ch == 43 || ch == 45){
            if(ch == 43) {
                _board->AddPace();
            } else if(ch == 45) {
                _board->ReducePace();
            }
            return;
        }
    }

    void GameLoop(){
        while(true){
            if(_status == GameStatus::GameOver){ //遊戲結束
                _view.ShowGameOver();
                break;
            }
            if(kbhit()) {
                Control();
            }
            _view.PrintBoard(_board); //印出分數板
            steady_clock::time_point now = steady_clock::now();
            if(duration_cast<milliseconds>(steady_clock::now() - Snstart).count() >= _board->GetRate()){
                Coordinate* c = new Coordinate();
                Move();
                Snstart = steady_clock::now(); // 重置時間
                _view.PrintSnake(_snakes, _eaten); //印出蛇
                _view.PrintApples(_apples); //印出蘋果
            }
        }
    }
};

int main(){
    SnakeGame* game = new SnakeGame();
    game->GameLoop();
    delete game;
    cin.get();
    return 0;
}