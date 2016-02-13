# short_script_v2
Q:What is this ?
A:short script v2です
# C++で使いたい
完成しないかもしれない
# .NETで使いたい
3ステップで簡単short script
- (1)参照にこのプロジェクトのDLLを追加する
- (2)ファイル名を渡してコンストラクトする
- (3)short scriptのソースを書く
#short scriptのソースの書き方
基本はLispライク
```
#コメントは#で
def main
let x 1#ローカル変数はletコマンド
let y 2
let z + 1 2#引数からはみ出した分ははみ出した分で一つのExprになる
println z
return 0#絶対何かしら返して欲しい
```
```
def func x y
return + x y

def main
for i 0 10#iは0から10まで動く
println func i 2
next#nextでfor文は終わり
return 0
```
```
def main
let x 10
while > x 0#while文
println x
loop#loopで終わり
return 0
```
```
async func x#コルーチンにするならdefじゃなくてasync
for i 0 x
yield_return i#処理を中断しつつ一旦値を返すならyield_return
next
return -1#処理を完全に終了させるならreturn

def main
foreach x func 10#現状foreach文以外でまともに使えない
println x
next
return 0
```

#ライセンス
Copyright (c) 2016, plasma-effect
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.