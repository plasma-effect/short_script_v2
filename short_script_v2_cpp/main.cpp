#include<stdio.h>
/*
●妥当そうな仮定
１．１日の終わりの時点でで、ひよこ・交流P・早熟・揚げが
　　１日の初めの時点より減っていてはならない
  ２．制圧→討伐→探索の優先度でAPを消費する
  ３．探索１回で交流P190。ボスは考慮しない
  ４．分クラの報酬厳選はしない（出したら倒す）
  ５．制圧・討伐・通常式・レア式の報酬割合はwiki準拠
  ●妥当じゃなさそうな仮定
  １．エーテル・名声は一切考慮しない。そのため救援はしない
  　　（エーテル・名声を考慮しない場合、僅差で救援＜探索）
	２．交流Pは、早熟が尽きるまで通常式、それ以降はレア式
	３．レア式で出る早熟・揚げは無視
	４．通常式においては必ず早熟にしてから旅立たせる
	　　つまり「平均に狐魂７＋揚げ２」は揚げが余っていてもしない
	  ５．通常式で早熟・揚げ持ちは狙わない（余りがあれば友情を狙う）
	  ６．通常式で早熟の個数が過小評価される傾向がある（後述）


	  ※早熟の個数の過小評価について
	  ひよこや親愛を旅立たせた後、「友情持ち３匹、その他５匹」が余ったとする
	  友情持ちの中に早熟タイプがいると期待でき、早熟の宝珠なしで１匹旅立たせられる
	  しかしこのプログラム中では「早熟が２個必要」と判定される（詳細略）
	  このように「余った最後の部分」の処理において、大きなズレが起こることがある
	  これがどの程度重大かは不明（１～５の影響の方が大きそう）
	  また、早熟が少なく揚げが多いパターン（SPの多くを制圧に回した場合）についても
	  誤差が大きくなる傾向にある
	  */

int choice[3] = {};//ひよこ,制圧周回回数,選択優先度
double max = 0;//心得期待値最大値
double t[2];//交流P消費関数return用。可否,心得値
const int k[5] = { 200,200,120,120,80 };//心得
const double se[9] = { 0,.330653,.29234,.258767,.229610,.204562,.183311,.165545,.150952 };//制圧報酬
const double g[6] = { .0275,.0365,.090,.006,.0135,.0075 };//通常式/ひよこ,親愛,友情,200,120,80
const double lg[6] = { .3322,0,0,.0970,.1352,.0344 };//レア式/ひよこ,親愛,友情,200,120,80
double min(double a, double b) { return a>b ? b : a; }

void kouryuushouhi(double kouryuup, double soujuku, double age, int hiyokokaishuu) {
	double kekka, tuujoukaisuu, reakaisuu, tabidatisuu, tabidatiritu;

	tabidatisuu = soujuku * 3 / 2;
	tuujoukaisuu = soujuku * 12 - age * 4;
	tabidatiritu = tabidatisuu / tuujoukaisuu;
	//旅立たせられるのは早熟*3/2体。素材込でその8倍の狐魂が必要。ただし揚げ1で4体減る
	//常に早熟*3>揚げなのでtuujoukaisuuは正
	//また、明らかにtabidatirituは0.125以上になる
	if (tuujoukaisuu * 200>kouryuup) { t[0] = 0;return; }//早熟消化のための交流Pが足りなければダメ
	kouryuup -= tuujoukaisuu * 200;//交流Pを減らす
	kouryuup += tuujoukaisuu * 1500 * g[1];
	kekka = tuujoukaisuu*(200 * g[3] + 120 * g[4] + 80 * g[5]);//心得と親愛はいつでも全て回収できる
															   //∵交流レアの心得期待値は40未満なので、心得か友情か選ぶなら心得であり
															   //友情を全て捨てればひよこまで全て回収できる
	tabidatiritu -= g[1] + g[3] + g[4] + g[5];
	reakaisuu = kouryuup / 2000;
	if ((reakaisuu + tuujoukaisuu*min(tabidatiritu, g[2]) / 2)*lg[0] >= hiyokokaishuu) {
		//ひよこ回収しなくてもたりるパターン
		reakaisuu += tuujoukaisuu*min(tabidatiritu, g[2]) / 2;
		t[0] = 1;
		t[1] = kekka + reakaisuu*(200 * lg[3] + 120 * lg[4] + 80 * lg[5]);
	}
	else if ((reakaisuu + tuujoukaisuu*min(tabidatiritu - min(tabidatiritu, g[0]), g[2]) / 2)*lg[0] >= hiyokokaishuu - tuujoukaisuu*min(tabidatiritu, g[0])) {
		//ひよこ回収してもだめなパターン
		t[0] = 0;return;
	}
	else {
		//いい感じに回収すれば大丈夫なパターン
		//ここに分岐する時点で、ひよこと友情の分配はどちらも上限に引っかからないので
		//素直に方程式を解くことによって厳密解を得られる
		//「(レア回数+友情作成数/2)*レアひよこ率+ひよこ作成数=ひよこ回収数」となればよく
		//xの方程式「(reakaisuu+x/2)*lg[0]+(tabidatiritu-x)=hiyokokaishuu」を解いて
		//「x=(lg[0]*reakaisuu+tabidatiritu-hiyokokaishuu)/(1-lg[0]/2)」を得るので
		reakaisuu += (lg[0] * reakaisuu + tabidatiritu - hiyokokaishuu) / (2 - lg[0]);
		t[0] = 1;
		t[1] = kekka + reakaisuu*(200 * lg[3] + 120 * lg[4] + 80 * lg[5]);
	}
	return;
}

double seiatukokoroe(int i1, int i2, int i3) {
	int i = 1, n = 0;
	double s = 0;
	for (;i<9;i++)if (i1 != i&&i2 != i&&i3 != i)s += k[n++] * se[i];
	return s;
}


int main() {

	double ap, toubatu, kyuuen, tansaku, kouryuup1, kouryuup2, soujuku1, soujuku2, age1, age2, kokoroe1, kokoroe2;
	int hiyoko, shuukai, i1, i2, i3;//i1,i2,i3は制圧での揚げ/早熟/親愛の選択優先度
	const int kyuuenmax = 0;//救援回数上限

	for (hiyoko = 0;hiyoko<30;hiyoko++) 
	{
		for (shuukai = 0;shuukai<50 && shuukai <= hiyoko * 2 + 12;shuukai++) 
		{//制圧
			ap = 2940 - shuukai * 60;//制圧分のAPを減らす
			toubatu = (hiyoko * 10 + 12 - shuukai * 5)*.5;//SPのあまりを討伐で消費したい
			if (toubatu * 3>ap)break;
			ap -= toubatu * 3;//討伐分のAPを減らす
			kyuuen = min(ap / 3, (double)kyuuenmax);//救援
			tansaku = (ap - kyuuen * 3) / 15 * 4 / 3;//救援分減らして残ったAPで探索
			age1 = toubatu*.0768;
			soujuku1 = toubatu*.0742;
			kouryuup1 = shuukai * 600 + toubatu * 50 + kyuuen * 50 + tansaku * 190;
			kokoroe1 = toubatu*(120 * (.0804 + .0806) + 80 * .0806);//討伐分の揚げ/早熟/心得/交流Pを計算
																	//			for(i1=1;i1<9;i1++){for(i2=1;i2<9;i2++){if(i1==i2)continue;for(i3=1;i3<9;i3++){if(i1==i3||i2==i3)continue;
			i1 = 1;
			i2 = 2;
			i3 = 3;
			age2 = age1 + se[i1] * shuukai;
			soujuku2 = soujuku1 + se[i2] * shuukai;
			kouryuup2 = kouryuup1 + se[i3] * shuukai * 1500;//優先度に応じて早熟/揚げ/親愛=交流Pを入手
			kouryuushouhi(kouryuup2, soujuku2, age2, hiyoko);//交流Pを消化。t[0]に可否,t[1]に心得期待値が返ってくる
			if (t[0] == 0)continue;//もしひよこが回収出来なかったら失敗
			kokoroe2 = kokoroe1 + t[1] + seiatukokoroe(i1, i2, i3)*shuukai;//交流Pと制圧分の心得を計算
			if (kokoroe2>max) 
			{//最高値を更新したならその情報を保管
				max = kokoroe2;
				choice[0] = hiyoko;
				choice[1] = shuukai;
				choice[2] = i1 * 100 + i2 * 10 + i3;
			}
			//			}}}
		}
		printf("hiyoko %2d shuukai %d sentaku %d kokoroe %5.4f\n", choice[0], choice[1], choice[2], max);
		max = 0;
	}
	//	printf("hiyoko %2d shuukai %d sentaku %d kokoroe %5.4f\n",choice[0],choice[1],choice[2],max);
	return 0;
}