package com.ghs.pumg.repository;

import com.ghs.pumg.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface UserRepository extends JpaRepository<User, Long> {

    Optional<User> findByIdAndPassword(String name, String password);
    Optional<User> findUserById(String userId);
}
