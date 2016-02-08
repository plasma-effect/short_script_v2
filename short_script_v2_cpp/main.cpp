#include<short_script/token_tree.hpp>

int main()
{
	typedef short_script_v2::utility::code_data<char> code_data;
	typedef short_script_v2::utility::basic_string<char> string;
	typedef short_script_v2::token_trait::token_tree<char> token_tree;

	string filename = "test.txt";
	auto token = short_script_v2::token_trait::make_token(
		short_script_v2::utility::make_code_data(0, 0, filename),
		"hoge");
	std::cout << (*short_script_v2::token_trait::get_token(token)) << std::endl;
}