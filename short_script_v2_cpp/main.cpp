#include<stdio.h>
/*
���Ó������ȉ���
�P�D�P���̏I���̎��_�łŁA�Ђ悱�E��P�E���n�E�g����
�@�@�P���̏��߂̎��_��茸���Ă��Ă͂Ȃ�Ȃ�
  �Q�D�������������T���̗D��x��AP�������
  �R�D�T���P��Ō�P190�B�{�X�͍l�����Ȃ�
  �S�D���N���̕�V���I�͂��Ȃ��i�o������|���j
  �T�D�����E�����E�ʏ펮�E���A���̕�V������wiki����
  ���Ó�����Ȃ������ȉ���
  �P�D�G�[�e���E�����͈�؍l�����Ȃ��B���̂��ߋ~���͂��Ȃ�
  �@�@�i�G�[�e���E�������l�����Ȃ��ꍇ�A�͍��ŋ~�����T���j
	�Q�D��P�́A���n���s����܂Œʏ펮�A����ȍ~�̓��A��
	�R�D���A���ŏo�鑁�n�E�g���͖���
	�S�D�ʏ펮�ɂ����Ă͕K�����n�ɂ��Ă��痷��������
	�@�@�܂�u���ςɌύ��V�{�g���Q�v�͗g�����]���Ă��Ă����Ȃ�
	  �T�D�ʏ펮�ő��n�E�g�������͑_��Ȃ��i�]�肪����ΗF���_���j
	  �U�D�ʏ펮�ő��n�̌����ߏ��]�������X��������i��q�j


	  �����n�̌��̉ߏ��]���ɂ���
	  �Ђ悱��e���𗷗���������A�u�F����R�C�A���̑��T�C�v���]�����Ƃ���
	  �F����̒��ɑ��n�^�C�v������Ɗ��҂ł��A���n�̕��Ȃ��łP�C������������
	  ���������̃v���O�������ł́u���n���Q�K�v�v�Ɣ��肳���i�ڍח��j
	  ���̂悤�Ɂu�]�����Ō�̕����v�̏����ɂ����āA�傫�ȃY�����N���邱�Ƃ�����
	  ���ꂪ�ǂ̒��x�d�傩�͕s���i�P�`�T�̉e���̕����傫�����j
	  �܂��A���n�����Ȃ��g���������p�^�[���iSP�̑����𐧈��ɉ񂵂��ꍇ�j�ɂ��Ă�
	  �덷���傫���Ȃ�X���ɂ���
	  */

int choice[3] = {};//�Ђ悱,���������,�I��D��x
double max = 0;//�S�����Ғl�ő�l
double t[2];//��P����֐�return�p�B��,�S���l
const int k[5] = { 200,200,120,120,80 };//�S��
const double se[9] = { 0,.330653,.29234,.258767,.229610,.204562,.183311,.165545,.150952 };//������V
const double g[6] = { .0275,.0365,.090,.006,.0135,.0075 };//�ʏ펮/�Ђ悱,�e��,�F��,200,120,80
const double lg[6] = { .3322,0,0,.0970,.1352,.0344 };//���A��/�Ђ悱,�e��,�F��,200,120,80
double min(double a, double b) { return a>b ? b : a; }

void kouryuushouhi(double kouryuup, double soujuku, double age, int hiyokokaishuu) {
	double kekka, tuujoukaisuu, reakaisuu, tabidatisuu, tabidatiritu;

	tabidatisuu = soujuku * 3 / 2;
	tuujoukaisuu = soujuku * 12 - age * 4;
	tabidatiritu = tabidatisuu / tuujoukaisuu;
	//������������̂͑��n*3/2�́B�f�ލ��ł���8�{�̌ύ����K�v�B�������g��1��4�̌���
	//��ɑ��n*3>�g���Ȃ̂�tuujoukaisuu�͐�
	//�܂��A���炩��tabidatiritu��0.125�ȏ�ɂȂ�
	if (tuujoukaisuu * 200>kouryuup) { t[0] = 0;return; }//���n�����̂��߂̌�P������Ȃ���΃_��
	kouryuup -= tuujoukaisuu * 200;//��P�����炷
	kouryuup += tuujoukaisuu * 1500 * g[1];
	kekka = tuujoukaisuu*(200 * g[3] + 120 * g[4] + 80 * g[5]);//�S���Ɛe���͂��ł��S�ĉ���ł���
															   //��𗬃��A�̐S�����Ғl��40�����Ȃ̂ŁA�S�����F��I�ԂȂ�S���ł���
															   //�F���S�Ď̂Ă�΂Ђ悱�܂őS�ĉ���ł���
	tabidatiritu -= g[1] + g[3] + g[4] + g[5];
	reakaisuu = kouryuup / 2000;
	if ((reakaisuu + tuujoukaisuu*min(tabidatiritu, g[2]) / 2)*lg[0] >= hiyokokaishuu) {
		//�Ђ悱������Ȃ��Ă������p�^�[��
		reakaisuu += tuujoukaisuu*min(tabidatiritu, g[2]) / 2;
		t[0] = 1;
		t[1] = kekka + reakaisuu*(200 * lg[3] + 120 * lg[4] + 80 * lg[5]);
	}
	else if ((reakaisuu + tuujoukaisuu*min(tabidatiritu - min(tabidatiritu, g[0]), g[2]) / 2)*lg[0] >= hiyokokaishuu - tuujoukaisuu*min(tabidatiritu, g[0])) {
		//�Ђ悱������Ă����߂ȃp�^�[��
		t[0] = 0;return;
	}
	else {
		//���������ɉ������Α��v�ȃp�^�[��
		//�����ɕ��򂷂鎞�_�ŁA�Ђ悱�ƗF��̕��z�͂ǂ��������Ɉ���������Ȃ��̂�
		//�f���ɕ��������������Ƃɂ���Č������𓾂���
		//�u(���A��+�F��쐬��/2)*���A�Ђ悱��+�Ђ悱�쐬��=�Ђ悱������v�ƂȂ�΂悭
		//x�̕������u(reakaisuu+x/2)*lg[0]+(tabidatiritu-x)=hiyokokaishuu�v��������
		//�ux=(lg[0]*reakaisuu+tabidatiritu-hiyokokaishuu)/(1-lg[0]/2)�v�𓾂�̂�
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
	int hiyoko, shuukai, i1, i2, i3;//i1,i2,i3�͐����ł̗g��/���n/�e���̑I��D��x
	const int kyuuenmax = 0;//�~���񐔏��

	for (hiyoko = 0;hiyoko<30;hiyoko++) 
	{
		for (shuukai = 0;shuukai<50 && shuukai <= hiyoko * 2 + 12;shuukai++) 
		{//����
			ap = 2940 - shuukai * 60;//��������AP�����炷
			toubatu = (hiyoko * 10 + 12 - shuukai * 5)*.5;//SP�̂��܂�𓢔��ŏ������
			if (toubatu * 3>ap)break;
			ap -= toubatu * 3;//��������AP�����炷
			kyuuen = min(ap / 3, (double)kyuuenmax);//�~��
			tansaku = (ap - kyuuen * 3) / 15 * 4 / 3;//�~�������炵�Ďc����AP�ŒT��
			age1 = toubatu*.0768;
			soujuku1 = toubatu*.0742;
			kouryuup1 = shuukai * 600 + toubatu * 50 + kyuuen * 50 + tansaku * 190;
			kokoroe1 = toubatu*(120 * (.0804 + .0806) + 80 * .0806);//�������̗g��/���n/�S��/��P���v�Z
																	//			for(i1=1;i1<9;i1++){for(i2=1;i2<9;i2++){if(i1==i2)continue;for(i3=1;i3<9;i3++){if(i1==i3||i2==i3)continue;
			i1 = 1;
			i2 = 2;
			i3 = 3;
			age2 = age1 + se[i1] * shuukai;
			soujuku2 = soujuku1 + se[i2] * shuukai;
			kouryuup2 = kouryuup1 + se[i3] * shuukai * 1500;//�D��x�ɉ����đ��n/�g��/�e��=��P�����
			kouryuushouhi(kouryuup2, soujuku2, age2, hiyoko);//��P�������Bt[0]�ɉ�,t[1]�ɐS�����Ғl���Ԃ��Ă���
			if (t[0] == 0)continue;//�����Ђ悱������o���Ȃ������玸�s
			kokoroe2 = kokoroe1 + t[1] + seiatukokoroe(i1, i2, i3)*shuukai;//��P�Ɛ������̐S�����v�Z
			if (kokoroe2>max) 
			{//�ō��l���X�V�����Ȃ炻�̏���ۊ�
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