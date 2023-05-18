package com.ghs.pumg.dto.response;

import com.ghs.pumg.entity.User;
import com.ghs.pumg.entity.UserAvatar;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

@Getter
@Setter
public class UserLoginRes extends DataResponseBody {
    @Getter
    @NoArgsConstructor
    static class Response {
        private Long idx;
        private String userId;

        private String userNickname;
        private Long worldCharacter;
        private Long healingCharacter;
        private String userTitle;

        public Response(User entity, Long worldCharacter, Long healingCharacter){
            this.idx = entity.getIdx();
            this.userId = entity.getId();
            this.userNickname = entity.getNickName();
            this.worldCharacter = worldCharacter;
            this.healingCharacter = healingCharacter;
            this.userTitle = entity.getTitle();
        }

    }
    public static UserLoginRes of(Integer statusCode, String message, User userInfo, List<UserAvatar> avatarInfo) {
        UserLoginRes res = new UserLoginRes();
        Long worldCharacter = null;
        Long healingCharacter = null;

        if (avatarInfo.size() == 1) {
            worldCharacter = avatarInfo.get(0).getAvatarIdx().getIdx();
        } else if(avatarInfo.size() == 2) {
            worldCharacter = avatarInfo.get(0).getAvatarIdx().getIdx();
            healingCharacter = avatarInfo.get(1).getAvatarIdx().getIdx();
        }

        Response response = new Response(userInfo, worldCharacter, healingCharacter);
        res.setStatus(statusCode);
        res.setMessage(message);
        res.getData().put("userInfo", response);
//        res.getData().put("access-token", accessToken);
//        res.getData().put("sellerIdx", idx);
        return res;
    }
}
