//ListBoxes를 표시할 내용을 저장
using UnityEngine;

/*목록 내용 컨테이너의 기본 클래스
 *이 클래스를 상속하여 개별 ListBank 생성
 */
public abstract class BaseListBank: MonoBehaviour
{
	public abstract string GetListContent(int index);
	public abstract int GetListLength();
}

public class ListBank : BaseListBank
{
	private int[] contents = {
		1, 2, 3, 4, 5, 6, 7, 8
	};

	public override string GetListContent(int index)
	{
		return contents[index].ToString();
	}

	public override int GetListLength()
	{
		return contents.Length;
	}
}
