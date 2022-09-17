import os
import csv

#csv_pathで指定されるcsvファイルに含まれる全ての文字をset_objにadd
def collect_letters_from_csv(set_obj, csv_path):
    with open(csv_path, "r", encoding="utf_8") as csv_file:
        for line in csv_file:
            for cell in line:
                for letter in cell:
                    set_obj.add(letter)

if __name__ == '__main__':
    #文字を格納するset
    letters_set = set()

    #./csv配下の全csvファイルにcollect_letters_from_csvを適用
    csv_dir = os.listdir("./csv")
    for csv_name in csv_dir:
        collect_letters_from_csv(letters_set, "./csv" + "/" + csv_name)

    #この段階でlettersに全ての文字が集められているはず
    #listに変換しソート
    letters_list = list(letters_set)
    letters_list.sort()

    #letters_listの文字をtxtファイルに出力
    with open("letters.txt", "w", encoding="utf_8", newline="") as f:
        for letter in letters_list:
            f.write(letter)