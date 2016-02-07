#include<short_script/utility.hpp>

int main()
{
	typedef short_script_v2::utility::basic_string<char> string;
	string str = "test";
	std::cout << str << std::endl;

	string str2;
	std::cout << str2 << std::endl;
	str2 = "test";
	std::cout << (str == str2) << std::endl;
}