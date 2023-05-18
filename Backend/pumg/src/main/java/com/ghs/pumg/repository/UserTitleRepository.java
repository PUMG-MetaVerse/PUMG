package com.ghs.pumg.repository;

import com.ghs.pumg.entity.Title;
import com.ghs.pumg.entity.User;
import com.ghs.pumg.entity.UserAvatar;
import com.ghs.pumg.entity.UserTitle;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface UserTitleRepository extends JpaRepository<UserTitle, Long> {

//    Optional<User> findByIdAndPassword(String name, String password);
//    Optional<UserAvatar> findUserById(String userId);
//    List<UserAvatar> findUserAvatarByUserIdx(User user);
    Optional<UserTitle> findByUserIdxAndTitleIdx(User user, Title title);
    List<UserTitle> findByUserIdx(User user);

}
