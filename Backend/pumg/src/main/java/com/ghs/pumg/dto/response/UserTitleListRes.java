package com.ghs.pumg.dto.response;

import com.ghs.pumg.entity.Ranking;
import com.ghs.pumg.entity.Title;
import com.ghs.pumg.entity.UserTitle;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.time.format.DateTimeFormatter;
import java.util.LinkedList;
import java.util.List;

@Getter
@Setter
public class UserTitleListRes extends DataResponseBody {
    @Getter
    @Setter
    @NoArgsConstructor
    public static class Response {
        private Long idx;
        private String title;
        private String description;
        private String getTime;

        public Response(Title entity, String getTime){
            this.idx = entity.getIdx();
            this.title = entity.getTitle();
            this.description = entity.getDescription();
            this.getTime = getTime;
        }
    }
    public static UserTitleListRes of(Integer statusCode, String message, List<UserTitle> titleInfo) {
        UserTitleListRes res = new UserTitleListRes();
        List<Response> response = new LinkedList<>();
        for (int i = 0; i < titleInfo.size(); i++) {
            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH시mm분");
            String time = titleInfo.get(i).getCreatedAt().format(formatter);
            response.add(new Response(titleInfo.get(i).getTitleIdx(), time));
        }
        res.setStatus(statusCode);
        res.setMessage(message);
        res.getData().put("titleInfo", response);
        return res;
    }
}
