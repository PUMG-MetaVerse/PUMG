package com.ghs.pumg.dto.response;

import com.ghs.pumg.entity.User;
import com.ghs.pumg.entity.UserAvatar;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
public class UserHealingCharacterRes extends DataResponseBody {
    @Getter
    @NoArgsConstructor
    static class Response {
        private Long idx;
        private Long avatarIdx;

        private Long userIdx;

        public Response(UserAvatar entity){
            this.idx = entity.getUserIdx().getIdx();
            this.avatarIdx = entity.getAvatarIdx().getIdx();
            this.userIdx = entity.getIdx();
        }
    }
    public static UserHealingCharacterRes of(Integer statusCode, String message, UserAvatar userInfo) {
        UserHealingCharacterRes res = new UserHealingCharacterRes();
        Response response = new Response(userInfo);
        res.setStatus(statusCode);
        res.setMessage(message);
        res.getData().put("userInfo", response);
//        res.getData().put("access-token", accessToken);
//        res.getData().put("sellerIdx", idx);
        return res;
    }
}
