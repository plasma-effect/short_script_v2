#pragma once

#include<short_script/utility.hpp>
#include<short_script/generic_data_type.hpp>

#include<iterator>
#include<string>
#include<memory>
#include<vector>

#include<boost/optional.hpp>

namespace short_script_v2
{
	using namespace generic_adt::place_holder;
	using generic_adt::generic_tag;
	using generic_adt::tuple;
	using generic_adt::type_tag;
	namespace token_trait
	{
		struct token_tree_t:generic_adt::generic_data_type<token_tree_t,
			tuple<utility::code_data<generic_tag>,std::basic_string<generic_tag>>,
			tuple<utility::code_data<generic_tag>, std::vector<token_tree_t>>>{};
		template<class CharType>using token_tree = std::shared_ptr<token_tree_t::value_type<CharType>>;
		const auto Token = token_tree_t::instance_function<0>();
		const auto Tree = token_tree_t::instance_function<1>();
		template<class CharType>token_tree<CharType> make_token(utility::code_data<CharType>data, std::basic_string<CharType> token)
		{
			return Token(type_tag<CharType>{})(data, std::move(token));
		}
		template<class CharType>token_tree<CharType> make_token(utility::code_data<CharType>data, CharType const* token)
		{
			return Token(type_tag<CharType>{})(data, token);
		}
		template<class CharType>token_tree<CharType> make_tree(utility::code_data<CharType>data, std::vector<token_tree<CharType>>&& tree)
		{
			return Tree(type_tag<CharType>{})(data, std::move(tree));
		}
		namespace detail
		{
			struct echo_t
			{
				template<class U, class T>T const&operator()(U const&, T const& v)const
				{
					return v;
				}
			};
		}

		const auto get_token = generic_adt::pattern_match::generic_match<std::basic_string<generic_tag> const&, token_tree_t>()
			| Token(i_, 0_) <= detail::echo_t{};
		const auto get_tree = generic_adt::pattern_match::generic_match<std::vector<token_tree_t> const&, token_tree_t>()
			| Tree(i_, 0_) <= detail::echo_t{};
		const auto get_data=generic_adt::pattern_match::generic_recursion<utility::code_data<generic_tag>,token_tree_t>()
			| Tree(0_, i_) <= [](auto const&, auto const&, auto const& t) {return t;}
			| Token(0_, i_) <= [](auto const&, auto const&, auto const& t) {return t;};
	}
}